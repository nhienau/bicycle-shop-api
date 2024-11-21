namespace api.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Dictionary<string, object>> GetZaloPayPaymentUrl();
    }
}
