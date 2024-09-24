namespace api.Models
{
    public class OrderDetail
    {
        public int? OrderId { get; set; }
        public Order? Order { get; set; }
        public int? ProductDetailId {  get; set; }
        public ProductDetail? ProductDetail { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
    }
}
