namespace api.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Status { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
