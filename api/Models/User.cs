namespace api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address {  get; set; } = string.Empty;
        public string PhoneNumber {  get; set; } = string.Empty;
        public bool Status { get; set; }
        public List<Cart> Carts { get; } = [];
        public List<ProductDetail> ProductDetails { get; } = [];
        public List<Order> Orders { get; set; } = [];
    }
}
