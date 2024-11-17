using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Product
{
    public class UpdateProductCategoryRequestDto
    {                
        public string? Name { get; set; }
        public bool? Status { get; set; }
    }
}
