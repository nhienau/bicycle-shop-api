using api.Dtos.ProductDetail;
using api.Dtos.Product;

namespace api.Dtos.Order
{
    public class OrderDetailDTO
    {
        public int? ProductDetailId { get; set; } // Mã chi tiết sản phẩm
        public int Quantity { get; set; } // Số lượng sản phẩm
        public long Price { get; set; } // Giá của sản phẩm
        public ProductDetailDto? ProductDetail { get; set; } // Thông tin chi tiết sản phẩm
        public ProductDTO? Product { get; set; } // Thông tin sản phẩm
    }
}
