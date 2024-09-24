namespace api.Models
{
    public class Cart
    {
        public int? UserId { get; set; }
        public User? User { get; set; }
        public int? ProductDetailId {  get; set; }
        public ProductDetail? ProductDetail { get; set; }
        public int Quantity { get; set; }
    }
}
