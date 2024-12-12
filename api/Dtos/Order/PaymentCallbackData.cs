namespace api.Dtos.Order
{
    public class PaymentCallbackData
    {
        public string Data { get; set; } = string.Empty;
        public string Mac {  get; set; } = string.Empty;
        public int Type { get; set; }
    }
}
