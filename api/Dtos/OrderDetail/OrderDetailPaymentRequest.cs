namespace api.Dtos.OrderDetail
{
    public class OrderDetailPaymentRequest
    {
        public int ProductDetailId { get; set; }
        public int Quantity { get; set; }
        public long? Price { get; set; }
    }
}
