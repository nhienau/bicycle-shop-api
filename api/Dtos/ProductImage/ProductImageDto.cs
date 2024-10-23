namespace api.Dtos.ProductImage
{
    public class ProductImageDto
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public int? ProductId { get; set; }
        public int? ProductDetailId { get; set; }
    }
}
