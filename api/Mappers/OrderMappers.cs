using api.Dtos.Order;
using api.Models;

namespace api.Mappers
{
    public static class OrderMapper
    {
        // Hàm chuyển từ CreateOrderDTO sang Order Entity
        public static Order ToOrderEntity(CreateOrderDTO createOrderDTO)
        {
            if (createOrderDTO == null) return null;

            return new Order
            {
                UserId = createOrderDTO.UserId,
                TotalPrice = createOrderDTO.TotalPrice,
                Address = createOrderDTO.Address,
                PhoneNumber = createOrderDTO.PhoneNumber,
                OrderDate = DateTime.Now, // Thiết lập ngày đặt hàng mặc định là thời điểm hiện tại
                Status = new OrderStatus { Id = 1 }, // Mặc định trạng thái là 'Chờ xác nhận' với Id = 1
            };
        }

        // Hàm chuyển từ OrderDetailDTO sang OrderDetail Entity
        public static OrderDetail ToOrderDetailEntity(OrderDetailDTO orderDetailDTO)
        {
            if (orderDetailDTO == null) return null;

            return new OrderDetail
            {
                ProductDetailId = orderDetailDTO.ProductDetailId,
                Quantity = orderDetailDTO.Quantity,
                Price = orderDetailDTO.Price
            };
        }

        // Hàm chuyển từ Order Entity sang OrderDTO
        public static OrderDTO ToOrderDTO(this Order order)
        {
            if (order == null) return null;

            OrderDTO dto = new OrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalPrice = order.TotalPrice,
                Address = order.Address,
                PhoneNumber = order.PhoneNumber,
                OrderDate = order.OrderDate,
                StatusName = order.Status?.Name ?? string.Empty,
            };

            if (order.OrderDetails != null)
            {
                dto.OrderDetails = order.OrderDetails.Select(ToOrderDetailDTO).ToList();
            }

            if (order.User != null)
            {
                dto.User = order.User.ToUserDto();
            }
            return dto;
        }

        // Hàm chuyển từ OrderDetail Entity sang OrderDetailDTO
        public static OrderDetailDTO ToOrderDetailDTO(this OrderDetail orderDetail)
        {
            if (orderDetail == null) return null;

            OrderDetailDTO dto = new OrderDetailDTO
            {
                ProductDetailId = orderDetail.ProductDetailId,
                Quantity = orderDetail.Quantity,
                Price = orderDetail.Price
            };

            if (orderDetail.ProductDetail != null)
            {
                dto.ProductDetail = orderDetail.ProductDetail.ToProductDetailDto1();
            }

            return dto;
        }
    }
}
