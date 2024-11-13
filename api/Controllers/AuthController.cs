using api.Dtos.User;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
                return Unauthorized("Invalid username or password.");
            }

            // Tạo JWT token nếu đăng nhập thành công
            var token = GenerateJwtToken(user);

            var response = new LoginResponseDTO
            {
                Username = user.Email,
                accessToken = token
            };

            return Ok(response);
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


        [HttpGet("userinfo")]
        [Authorize] // yêu cầu người dùng đã đăng nhập
        public async Task<IActionResult> GetUserInfo()
        {
            // Lấy JWT từ header Authorization
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            int userId = GetUserIdFromToken(token);
            //int userId = Convert.ToInt32(HttpContext.User.FindFirstValue("userID"));


            if (userId == 0)
            {
                return Unauthorized("Invalid token.");
            }

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
                // Thêm các thông tin khác nếu cần
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
                Address = registerDto.FullName,
                PhoneNumber = registerDto.FullName,
                Password = registerDto.Password // Chưa băm, sẽ được băm trong repository
            };

            await _userRepository.RegisterUserAsync(user);

            return Ok("User registered successfully!");
        }
    }
}
