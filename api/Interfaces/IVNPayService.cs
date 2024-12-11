using api.Dtos.VNPay;

namespace api.Interfaces
{
    public interface IVNPayService
    {
        string CreatePaymentUrl(PaymentInfo model, HttpContext context);
        PaymentResponse PaymentExecute(IQueryCollection collections);

    }
}
