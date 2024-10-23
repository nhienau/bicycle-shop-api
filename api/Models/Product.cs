namespace api.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? ProductCategoryId { get; set; }
        public ProductCategory? ProductCategory { get; set; }
        public bool Status { get; set; }
        public List<ProductDetail> ProductDetails { get; } = [];
        public List<ProductImage> ProductImages { get; } = [];
    }
}
