using api.Dtos.ProductDetail;
using api.Dtos.Product;

namespace api.Dtos.Order
{
  public class OrderDTO
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public long TotalPrice { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string StatusName { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderDetailDTO> OrderDetails { get; set; }
}

}