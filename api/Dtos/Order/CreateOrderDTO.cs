
namespace api.Dtos.Order;
public class CreateOrderDTO
{
    public int? UserId { get; set; }
    public long TotalPrice { get; set; }
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int StatusId { get; set; }
    public List<OrderDetailDTO> OrderDetails { get; set; } = new();
}
