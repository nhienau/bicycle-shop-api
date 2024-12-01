using System.ComponentModel.DataAnnotations;

namespace api.Dtos.ProductCategory
{
    public class CreateProductCategoryRequestDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public bool Status { get; set; }
    }
}
