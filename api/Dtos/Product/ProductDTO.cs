using api.Models;

namespace api.Dtos.Product
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public long Price { get; set; }
        public bool Status { get; set; }
    }
}
