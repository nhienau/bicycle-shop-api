using api.Dtos.Order;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Utilities;
using Microsoft.EntityFrameworkCore;
using api.Controllers;
using api.Data;

namespace api.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDBContext _context;
        public OrderRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        // List tất cả Orders
public async Task<List<OrderDTO>> GetAllOrdersWithDetailsAsync(string? productName = null)
{
    // Lấy tất cả các đơn hàng cùng với OrderDetails và ProductDetails
    var ordersQuery = _context.Orders
        .Include(o => o.OrderDetails)
            .ThenInclude(od => od.ProductDetail)
        .Include(o => o.Status)
        .AsQueryable();

    // Nếu productName có giá trị, lọc theo tên sản phẩm
    if (!string.IsNullOrEmpty(productName))
    {
        ordersQuery = ordersQuery.Where(o => o.OrderDetails
            .Any(od => EF.Functions.Like(od.ProductDetail.Product.Name.Trim().ToLower(), $"%{productName}%")));
    }

    // Thực hiện truy vấn và lấy danh sách các đơn hàng
    var orders = await ordersQuery.ToListAsync();

    // Chuyển đổi danh sách các đơn hàng thành DTO
    return orders.Select(OrderMapper.ToOrderDTO).ToList();
}


        // List tất cả Orders của một User cụ thể
        public async Task<List<OrderDTO>> GetOrdersByUserIdWithDetailsAsync(int userId, string? productName = null)
        {
            var ordersQuery = _context.Orders
        .Where(o => o.UserId == userId)
        .Include(o => o.OrderDetails)
            .ThenInclude(od => od.ProductDetail)
        .Include(o => o.Status)
        .AsQueryable();

            // Nếu productName có giá trị, thì lọc theo tên sản phẩm
            if (!string.IsNullOrEmpty(productName))
    {
        ordersQuery = ordersQuery.Where(o => o.OrderDetails
            .Any(od => EF.Functions.Like(od.ProductDetail.Product.Name.Trim().ToLower(), $"%{productName}%")));
    }

            var orders = await ordersQuery.ToListAsync();

            // Chuyển đổi các đơn hàng thành DTO và trả về kết quả
            return orders.Select(OrderMapper.ToOrderDTO).ToList();
        }

        // Thêm mới một Order
        public async Task<OrderDTO> AddOrderAsync(CreateOrderDTO newOrder)
        {
            // Đảm bảo trạng thái mặc định là "Chờ xác nhận" nếu không được chỉ định
            var status = await _context.OrderStatuses.FindAsync(newOrder.StatusId) ??
                         await _context.OrderStatuses.FindAsync(1);

            var order = new Order
            {
                UserId = newOrder.UserId,
                Address = newOrder.Address,
                PhoneNumber = newOrder.PhoneNumber,
                TotalPrice = newOrder.TotalPrice,
                OrderDate = DateTime.UtcNow,
                Status = status
            };

            // Thêm OrderDetails
            foreach (var item in newOrder.OrderDetails)
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    ProductDetailId = item.ProductDetailId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            // Thêm Order vào database
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return OrderMapper.ToOrderDTO(order);
        }
        public async Task<PaginatedResponse<OrderDTO>> GetAllAsync(OrderQueryDTO query)
        {
            //IQueryable<Order> Orders= _context.Orders.Include(c => c.ProductDetail).ThenInclude(p => p.Orders).AsQueryable();

            //if (!string.IsNullOrWhiteSpace(query.Name))
            //{
            //    products = products.Where(p => p.Name.Contains(query.Name));
            //}
            IQueryable<Order> Orders = _context.Orders.Include(c => c.ProductDetails).AsQueryable();

            int totalElements = await Orders.CountAsync();

            int recordsSkipped = (query.PageNumber - 1) * query.PageSize;

            var result = await Orders.Skip(recordsSkipped).Take(query.PageSize).ToListAsync();
            var resultDto = result.Select(c => c.ToOrderDTO()).ToList();

            return new PaginatedResponse<OrderDTO>(resultDto, query.PageNumber, query.PageSize, totalElements);
        }

        public async Task<PaginatedResponse<OrderDTO>> GetOrderByUserIdAsync(int userId, OrderQueryDTO query)
        {
            //return _context.Orders.FirstOrDefaultAsync(i => i.UserId == id);
            IQueryable<Order> Orders = _context.Orders.Where(c => c.UserId == userId).Include(c => c.ProductDetails).ThenInclude(pd => pd.Product).AsQueryable();
            int totalElements = await Orders.CountAsync();
            int recordsSkipped = (query.PageNumber - 1) * query.PageSize;

            var result = await Orders.Skip(recordsSkipped).Take(query.PageSize).ToListAsync();
            var resultDto = result.Select(c => c.ToOrderDTO()).ToList();

            return new PaginatedResponse<OrderDTO>(resultDto, query.PageNumber, query.PageSize, totalElements);
            //return _context.Orders.Include(c => c.ProductDetail).ThenInclude(pd => pd.Product).FirstOrDefaultAsync(i => i.UserId == userId);
        }
    }
}
