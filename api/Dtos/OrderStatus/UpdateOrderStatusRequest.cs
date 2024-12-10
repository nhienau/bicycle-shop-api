namespace api.Dtos.OrderStatus
{
    public class UpdateOrderStatusRequest
    {
        public int OrderId { get; set; }
        public string StatusName { get; set; } = string.Empty;
    }
}
