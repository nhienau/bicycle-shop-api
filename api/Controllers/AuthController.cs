using api.Dtos.User;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Net.WebSockets;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public AuthController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDTO request)
        {
            var user = _userRepository.GetUserByUserEmail(request.Email);

            // Kiểm tra nếu người dùng không tồn tại hoặc mật khẩu không đúng
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized("Invalid email or password.");
            }

            var accessToken = GenerateJwtToken(user);

            // Tạo Access Token ngắn hạn
            //var accessToken = GenerateAccessToken(user);

            // Tạo Refresh Token dài hạn
            //var refreshToken = GenerateRefreshToken();

            // Lưu Refresh Token vào database thông qua Repository
            //_userRepository.SaveRefreshToken(user.Id, refreshToken);

            // Trả về Access Token và Refresh Token
            var response = new LoginResponseDTO
            {
                Email = user.Email,
                AccessToken = accessToken,
            };

            return Ok(response);
        }

        [HttpPost("refresh")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequestDTO request)
        {
            var user = _userRepository.GetUserByUserEmail(request.Email);

            if (user == null || !_userRepository.ValidateRefreshToken(user.Id, request.RefreshToken))
            {
                return Unauthorized("Invalid refresh token.");
            }

            var newAccessToken = GenerateAccessToken(user);

            return Ok(new { AccessToken = newAccessToken });
        }


        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateAccessToken(User user)
        {
            var claims = new[]
            {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(15), // Access Token có thời hạn 15 phút
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString(); // Hoặc sử dụng phương pháp sinh token an toàn hơn
        }


        [HttpGet("userinfo")]
        [Authorize] // yêu cầu người dùng đã đăng nhập
        public async Task<IActionResult> GetUserInfo()
        {
            // Lấy JWT từ header Authorization
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            int userId = GetUserIdFromToken(token);


            if (userId == 0)
            {
                return Unauthorized("Invalid token.");
            }

            // Lấy thông tin userId từ HttpContext, không cần giải mã token thủ công
            //var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "nameid" || c.Type == JwtRegisteredClaimNames.Sub);

            //if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            //{
            //    return Unauthorized("Invalid token or userId not found.");
            //}

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Trả về thông tin người dùng, có thể thêm dữ liệu cần thiết vào response
            var userInfo = new
            {
                userID = user.Id,
                userEmail = user.Email,
                user.Name,
                user.Address,
                user.PhoneNumber,
            };

            return Ok(userInfo);
        }

        private int GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
            var userIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "nameid" || c.Type == JwtRegisteredClaimNames.Sub);

            if (userIdClaim == null)
            {
                return 0; // hoặc giá trị nào đó để thể hiện rằng không tìm thấy Id
            }

            if (int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            else
            {
                throw new InvalidOperationException("User ID in token is not a valid integer.");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerDto)
        {
            // Kiểm tra email đã tồn tại
            var existingUser = await _userRepository.GetUserByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return BadRequest("Email is already taken.");
            }

            // Tạo người dùng mới
            var user = new User
            {
                Email = registerDto.Email,
                Name = registerDto.FullName,
                Address = registerDto.Address,
                PhoneNumber = registerDto.PhoneNumber,
                Password = registerDto.Password, // Chưa băm, sẽ được băm trong repository
                Status = true
            };

            await _userRepository.RegisterUserAsync(user);

            return Ok("User registered successfully!");
        }

        [HttpPost("logout")]
        [Authorize] // Chỉ người dùng đã đăng nhập mới có thể đăng xuất
        public IActionResult Logout()
        {
            // Trả về phản hồi OK để yêu cầu frontend xóa token
            return Ok("Logged out successfully.");
        }

        [HttpPost("request-otp")]
        public async Task<IActionResult> RequestOtp([FromBody] string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Generate OTP
            var otp = new Random().Next(100000, 999999).ToString();
            var expirationTime = DateTime.UtcNow.AddMinutes(1); // Thời gian hiệu lực là 1 phút
            
            //  Mã hóa mã OTP và thời gian hết hạn
            var encryptedOtpToken = GenerateOtpToken(otp, expirationTime);

            // Send OTP to user's email
            SendEmail(user.Email, "Mã OTP Của Bạn", $"Mã OTP xác thực để thay đổi mật khẩu đã quên là {otp}, hiệu lực 1 phút.");

            //return Ok("OTP sent to your email.");
            return Ok(new
            {
                OtpToken = encryptedOtpToken,
                ExpirationTime = expirationTime
            });
        }

        private string GenerateOtpToken(string otp, DateTime expirationTime)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]); // Khóa mã hóa từ appsettings.json

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim("otp", otp),
            new Claim("expires", expirationTime.ToString())
                }),
                Expires = expirationTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        //[HttpPost("validate-otp")]
        //public async Task<IActionResult> ValidateOtp(int userId, string otp)
        //{
        //    bool isValid = await _userRepository.ValidateOtpAsync(userId, otp);
        //    if (!isValid)
        //    {
        //        return Unauthorized("Invalid or expired OTP.");
        //    }

        //    // Process to reset password (e.g., redirect to reset password page)
        //    return Ok("OTP validated. Proceed with password reset.");
        //}

        private void SendEmail(string email, string subject, string message)
        {
            var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
            {
                Port = int.Parse(_configuration["Smtp:Port"]),
                Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
                EnableSsl = bool.Parse(_configuration["Smtp:EnableSsl"])
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Smtp:Username"]),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);

            smtpClient.Send(mailMessage);
        }

        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            try
            {
                tokenHandler.ValidateToken(request.OtpToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var otp = jwtToken.Claims.First(c => c.Type == "otp").Value;
                var expirationTime = DateTime.Parse(jwtToken.Claims.First(c => c.Type == "expires").Value);

                if (expirationTime < DateTime.UtcNow)
                {
                    return BadRequest("OTP has expired.");
                }

                if (otp != request.EnteredOtp)
                {
                    return BadRequest("Invalid OTP.");
                }

                return Ok("OTP verified successfully.");
            }
            catch
            {
                return Unauthorized("Invalid OTP token.");
            }
        }
    }
}
