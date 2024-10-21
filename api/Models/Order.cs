namespace api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public long TotalPrice { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public OrderStatus? Status { get; set; }
        public List<ProductDetail> ProductDetails { get; } = [];
        public List<OrderDetail> OrderDetails { get; } = [];
    }
}
