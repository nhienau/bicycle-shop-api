namespace api.Models
{
    public class ProductDetail
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
        public string Size { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public long Price { get; set; }
        public int Quantity { get; set; }
        public bool Status { get; set; }
        public List<ProductImage> ProductImages { get; } = [];
        public List<Cart> Carts { get; } = [];
        public List<User> Users { get; } = [];
        public List<Order> Orders { get; } = [];
        public List<OrderDetail> OrderDetails { get; } = [];
    }
}
