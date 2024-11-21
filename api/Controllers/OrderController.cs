using api.Dtos.Order;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _OrderRepo;

        public OrderController(IOrderRepository orderRepo)
        {
            _OrderRepo = orderRepo;
        }

        // List tất cả Orders
        [HttpGet("all-orders")]
        public async Task<IActionResult> GetAllOrders([FromQuery] string? productName)
        {
            var orders = await _OrderRepo.GetAllOrdersWithDetailsAsync(productName);
            return Ok(orders);
        }


        // List tất cả Orders của một User cụ thể
        [HttpGet("user-orders/{userId}")]
        public async Task<IActionResult> GetUserOrders([FromRoute] int userId, [FromQuery] string? productName = null)
        {
            var orders = await _OrderRepo.GetOrdersByUserIdWithDetailsAsync(userId, productName);

            return Ok(orders);
        }

        // Thêm mới một Order
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] CreateOrderDTO newOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _OrderRepo.AddOrderAsync(newOrder);
            return CreatedAtAction(nameof(GetUserOrders), new { userId = order.UserId }, order);
        }
    }
}
