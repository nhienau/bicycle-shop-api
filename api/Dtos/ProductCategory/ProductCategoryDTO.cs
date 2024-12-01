using api.Dtos.ProductDetail;
using api.Dtos.Cart;

namespace api.Dtos.Product
{
    public class ProductCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
}
