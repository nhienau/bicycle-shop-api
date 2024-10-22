using System.ComponentModel.DataAnnotations;

namespace api.Dtos.ProductDetail
{
    public class ProductDetailRequestDto
    {
        public string Size { get; set; } = string.Empty;
        [Required]
        public string Color { get; set; } = string.Empty;
        [Required]
        public long Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}
