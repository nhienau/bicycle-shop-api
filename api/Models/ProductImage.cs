namespace api.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public int? ProductDetailId { get; set; }
        public ProductDetail? ProductDetail { get; set; }
    }
}
