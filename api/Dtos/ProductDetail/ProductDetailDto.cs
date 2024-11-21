using api.Dtos.ProductImage;
using api.Models;
using api.Dtos.Cart;
using api.Dtos.Product;

namespace api.Dtos.ProductDetail
{
    public class ProductDetailDto
    {
        public int Id { get; set; }
        public string Size { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public long Price { get; set; }
        public int Quantity { get; set; }
        public bool Status { get; set; }
        public int? ProductId { get; set; }
        public ProductImageDto? ProductImage { get; set; }
        public ProductDTO? Product { get; set; }
    }
}
