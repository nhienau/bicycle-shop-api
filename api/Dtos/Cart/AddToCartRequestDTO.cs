namespace api.Dtos.Cart
{
    public class AddToCartRequestDTO
    {
        public int UserId { get; set; }
        public int ProductDetailId { get; set; }
        public int Quantity { get; set; }
    }
}
