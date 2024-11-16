using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Product
{
    public class UpdateProductCategoryRequestDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
}
