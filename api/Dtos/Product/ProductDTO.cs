using api.Dtos.ProductCategory;
using api.Dtos.ProductDetail;
using api.Dtos.ProductImage;
using api.Dtos.Cart;

namespace api.Dtos.Product
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Status { get; set; }
        public ProductCategoryDto? ProductCategory { get; set; }
        public List<ProductDetailDto> ProductDetails { get; set; } = [];
        public List<ProductImageDto> ProductImages { get; set; } = [];
    }
}
