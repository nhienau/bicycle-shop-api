using api.Dtos.Order;

namespace api.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Dictionary<string, object>> GetZaloPayPaymentUrl(OrderPaymentRequest req);
    }
}
