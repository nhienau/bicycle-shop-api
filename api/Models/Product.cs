namespace api.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public long Price { get; set; }
        public int? ProductCategoryId { get; set; }
        public ProductCategory? ProductCategory { get; set; }
        public bool Status { get; set; }
        public List<Cart>? Carts { get; }
        public List<User> Users { get; } = [];
        public List<Order> Orders { get; } = [];
        public List<OrderDetail> OrderDetails { get; } = [];

    }
}
