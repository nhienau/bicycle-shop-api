using api.Dtos.Order;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Utilities;
using Microsoft.EntityFrameworkCore;
using api.Controllers;
using api.Data;
using api.Dtos.OrderStatus;

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
            string statusName = query.StatusName;
            string customerName = query.CustomerName;
            string isoFromDate = query.FromDate;
            string isoToDate = query.ToDate;
            int? userId = query.UserId;

            IQueryable<Order> orders = _context.Orders
                .Include(c => c.User)
                .Include(c => c.Status)
                .AsQueryable();

            if (userId.HasValue)
            {
                orders = orders.Where(o => o.User.Id == userId);
            }

            if (!string.IsNullOrEmpty(statusName))
            {
                orders = orders.Where(o => o.Status.Name == statusName);
            }

            if (!string.IsNullOrEmpty(customerName))
            {
                orders = orders.Where(o => o.User.Name.ToLower().Contains(customerName.ToLower()));
            }

            if (!string.IsNullOrEmpty(isoFromDate) && !string.IsNullOrEmpty(isoToDate))
            {
                DateTime fromDate = DateTime.Parse(isoFromDate);
                DateTime toDate = DateTime.Parse(isoToDate);
                orders = orders.Where(o => o.OrderDate >= fromDate && o.OrderDate <= toDate);
            }

            orders = orders.OrderByDescending(o => o.OrderDate);

            int totalElements = await orders.CountAsync();

            int recordsSkipped = (query.PageNumber - 1) * query.PageSize;

            var result = await orders.Skip(recordsSkipped).Take(query.PageSize).ToListAsync();
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

        public async Task<Order?> GetOrderById(int id)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.Status)
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductDetail)
                        .ThenInclude(pd => pd.Product)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductDetail)
                        .ThenInclude(pd => pd.ProductImage)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order?> UpdateOrderStatus(UpdateOrderStatusRequest req)
        {
            Order? order = await _context.Orders
                .Include(o => o.Status)
                .FirstOrDefaultAsync(o => o.Id == req.OrderId);

            if (order == null)
            {
                return null;
            }

            OrderStatus? status = await _context.OrderStatuses.FirstOrDefaultAsync(o => o.Name == req.StatusName);
            if (status == null)
            {
                return null;
            }
            if (order.Status?.Id == status.Id)
            {
                return null;
            }
            order.Status = status;
            await _context.SaveChangesAsync();
            return order;
        }
    }
}
