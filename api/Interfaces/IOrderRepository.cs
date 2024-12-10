using api.Dtos.Order;
using api.Dtos.OrderStatus;
using api.Models;
using api.Utilities;

namespace api.Interfaces
{
    public interface IOrderRepository
    {
        Task<PaginatedResponse<OrderDTO>> GetAllAsync(OrderQueryDTO query);
        Task<PaginatedResponse<OrderDTO>> GetOrderByUserIdAsync(int userId, OrderQueryDTO query);
        Task<List<OrderDTO>> GetAllOrdersWithDetailsAsync(string? productName = null);
        Task<List<OrderDTO>> GetOrdersByUserIdWithDetailsAsync(int userId, string? productName = null);

        // Sửa lại kiểu dữ liệu cho đúng
        Task<OrderDTO> AddOrderAsync(CreateOrderDTO newOrder);
        Task<Order?> GetOrderById(int id);
        Task<Order?> UpdateOrderStatus(UpdateOrderStatusRequest req);
    }
}
