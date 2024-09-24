using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Product
{
    public class CreateProductRequestDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(1, long.MaxValue)]
        public long Price { get; set; }
        public int ProductCategoryId { get; set; }
    }
}
