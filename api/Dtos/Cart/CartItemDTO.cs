namespace api.Dtos.Cart
{
    public class CartItemDTO
    {
        public int productId { get; set; }
        public int ProductDetailId { get; set; }
        public int quantity { get; set; }
        public string name { get; set; } = string.Empty;
        public string size { get; set; } = string.Empty;
        public string color { get; set; } = string.Empty;
        public long price { get; set; }
        //public int QuantityStock { get; set; }
        public string image { get; set; } = string.Empty;
    }
}
