namespace api.Models
{
    public class Cart
    {
        public int? UserId { get; set; }
        public int? ProductDetailId {  get; set; }
        public ProductDetail? ProductDetail { get; set; }
        //public int? ProductId { get; set; }
        //public Product? Product { get; set; }
        public int Quantity { get; set; }

        //public List<ProductDetail> ProductDetails { get; } = [];
        //public object ProductDetails { get; internal set; }
        //public string AnonymousId { get; internal set; }
    }
}
