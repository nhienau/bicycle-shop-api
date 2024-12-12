using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Product
{
    public class CreateProductRequestDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public int ProductCategoryId { get; set; }
    }
}
