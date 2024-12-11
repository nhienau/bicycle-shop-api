using api.Dtos.Order;
using api.Dtos.OrderStatus;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Utilities;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X9;

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

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] OrderQueryDTO query)
        {
            PaginatedResponse<OrderDTO> orders = await _OrderRepo.GetAllAsync(query);
            return Ok(orders);
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrderById([FromRoute] int id)
        {
            Order? order = await _OrderRepo.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order.ToOrderDTO());
        }

        [HttpPatch("status")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusRequest req)
        {
            Order? order = await _OrderRepo.UpdateOrderStatus(req);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order.ToOrderDTO());
        }

        [HttpPost("create")]
        public async Task<IActionResult> GetZaloPayPaymentUrl([FromBody] OrderPaymentRequest request)
        {
            Order order = await _OrderRepo.CreateOrderAsync(request);
            return Ok(order.ToOrderDTO());
        }
    }
}
