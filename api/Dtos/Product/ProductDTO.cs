using api.Models;

namespace api.Dtos.Product
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
}
