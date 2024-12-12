namespace api.Dtos.Order
{
    public class PaymentCallbackResponse
    {
        public bool Success { get; set; }
        public OrderDTO? Order { get; set; }
    }
}
