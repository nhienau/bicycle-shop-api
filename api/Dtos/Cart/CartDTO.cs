using api.Dtos.ProductDetail;
using api.Dtos.Product;

namespace api.Dtos.Cart
{
    public class CartDTO
    {
        public int? UserId { get; set; }                 // Nullable, nếu người dùng đã đăng nhập thì có UserId
        //public string AnonymousId { get; set; }
        public int? ProductDetailId { get; set; }

        public int Quantity { get; set; }
        public ProductDetailDto ProductDetail { get; set; }

        //public List<ProductDetailDto> ProductDetails { get; set; } = [];
        //public ProductDetailDto ProductDetail { get; set; } = new ProductDetailDto();
        //public ProductDetailDto ProductDetails { get; set; }

        //public List<ProductDetailDto> ProductDetails { get; set; } = {};


    }
}
