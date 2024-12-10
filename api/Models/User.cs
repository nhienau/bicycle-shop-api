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
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public bool Status { get; set; }
        //public ICollection<Cart> Carts { get; set; }
        public List<ProductDetail> ProductDetails { get; } = [];
        public List<Order> Orders { get; set; } = [];

        //public string Otp { get; set; }
        //public DateTime? OtpExpiration { get; set; }
    }
}
