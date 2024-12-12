using api.Dtos.OrderDetail;

namespace api.Dtos.Order
{
    public class OrderPaymentRequest
    {
        public int UserId { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public long? TotalPrice { get; set; }
        public List<OrderDetailPaymentRequest> OrderDetail { get; set; } = new List<OrderDetailPaymentRequest>();
    }
}
