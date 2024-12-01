using api.Dtos.Cart;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Repositories;
using api.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartRepository _cartRepo;
        public CartsController(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CartQueryDTO query)
        {
            PaginatedResponse<CartDTO> list = await _cartRepo.GetAllAsync(query);
            return Ok(list);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCartById([FromRoute] int id, [FromQuery] CartQueryDTO query)
        {
            PaginatedResponse<CartDTO> carts = await _cartRepo.GetCartByUserIdAsync(id, query);
            if (carts == null)
            {
                return NotFound();
            }
            return Ok(carts);
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SyncCart([FromBody] List<CartItemDTO> clientCart)
        {
            // Lấy UserId từ token hoặc context
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = GetUserIdFromToken(token);

            if (userId == null)
            {
                return Unauthorized("User is not logged in.");
            }

            await _cartRepo.SyncCartAsync(userId, clientCart);

            // Trả về giỏ hàng đồng bộ
            var updatedCart = await _cartRepo.GetCartByUserIdAsync(userId);
            return Ok(updatedCart);
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

        [HttpGet("get")]
        [Authorize]
        public async Task<IActionResult> GetCart()
        {
            // Lấy UserId từ token
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = GetUserIdFromToken(token);

            if (userId == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var cartItems = await _cartRepo.GetCartByUserIdAsync(userId);
            return Ok(cartItems);
        }

        [HttpPost("sync-and-get")]
        [Authorize]
        public async Task<IActionResult> SyncAndGetCart([FromBody] List<CartItemDTO> clientCart)
        {
            // Lấy UserId từ token
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = GetUserIdFromToken(token);

            if (userId == null)
            {
                return Unauthorized("User is not logged in.");
            }

            // Đồng bộ giỏ hàng
            await _cartRepo.SyncCartAsync(userId, clientCart);

            // Lấy toàn bộ giỏ hàng đã đồng bộ
            var updatedCart = await _cartRepo.GetCartByUserIdAsync(userId);
            return Ok(updatedCart);
        }

        [HttpPost("addCart")]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDTO cartItem)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = GetUserIdFromToken(token);
            await _cartRepo.AddToCartAsync(userId, cartItem);
            await _cartRepo.SaveChangesAsync();
            return Ok(new { message = "Product added to cart successfully" });
        }

        [HttpDelete("delete/{productDetailId}")]
        [Authorize]

        public async Task<IActionResult> RemoveFromCart(int productDetailId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = GetUserIdFromToken(token);
            await _cartRepo.RemoveFromCartAsync(userId, productDetailId);
            await _cartRepo.SaveChangesAsync();
            return Ok(new { message = "Product removed from cart successfully" });
        }
    }
}
