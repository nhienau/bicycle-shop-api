using api.Dtos.Order;
using api.Dtos.OrderStatus;
using api.Interfaces;
<<<<<<< HEAD
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
=======
using api.Mappers;
using api.Models;
using api.Utilities;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X9;
>>>>>>> d9c5bb2a565d53e1a0bb25ba74b81c0a90bfc2bd

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

        [HttpGet("user-orders")]
        [Authorize]
        public async Task<IActionResult> GetUserOrdersFromToken([FromQuery] string? productName = null)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            int userId = GetUserIdFromToken(token);
            var orders = await _OrderRepo.GetOrdersByUserIdWithDetailsAsync(userId, productName);

            return Ok(orders);
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
