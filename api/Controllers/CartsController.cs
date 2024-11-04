using api.Dtos.Cart;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Repositories;
using api.Utilities;
using Microsoft.AspNetCore.Mvc;

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
        //public async Task<ActionResult<List<CartDTO>>> GetAllCarts([FromQuery] int? userId, [FromQuery] string anonymousId)
        //{
        //    // Kiểm tra xem cả userId và anonymousId đều không có thì trả về lỗi
        //    if (userId == null && string.IsNullOrEmpty(anonymousId))
        //    {
        //        return BadRequest("UserId or AnonymousId is required.");
        //    }

        //    // Gọi repository để lấy giỏ hàng
        //    var carts = await _cartRepo.GetAllCartsAsync(userId, anonymousId);

        //    // Kiểm tra nếu giỏ hàng trống
        //    if (carts == null || carts.Count == 0)
        //    {
        //        return NotFound("No items found in the cart.");
        //    }

        //    return Ok(carts); // Trả về danh sách giỏ hàng
        //}
        //public async Task<IActionResult> GetAll()
        //{
        //    List<CartDTO> carts= await _cartRepo.GetAllCartsAsync();
        //    return Ok(carts);
        //}
    }
}
