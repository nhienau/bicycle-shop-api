using System.ComponentModel.DataAnnotations;

namespace api.Dtos.ProductCategory
{
    public class UpdateProductCategoryRequestDto
    {                
        public string? Name { get; set; }
        public bool? Status { get; set; }
    }
}
