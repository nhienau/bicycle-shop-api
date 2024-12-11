using api.Configurations;
using api.Dtos.VNPay;
using api.Interfaces;
using api.Utilities;

namespace api.Services
{
    public class VNPayService : IVNPayService
    {
        private readonly VNPayConfig _vnPayConfig;
        private readonly IConfiguration _configuration;

        public VNPayService(VNPayConfig vnPayConfig, IConfiguration configuration)
        {
            _vnPayConfig = vnPayConfig;
            _configuration = configuration;
        }
        public string CreatePaymentUrl(PaymentInfo model, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VNPay();
            var callbackUrl = _vnPayConfig.CallbackUrl;
            var expiredTime = timeNow.AddMinutes(15);

            pay.AddRequestData("vnp_Version", _vnPayConfig.Version);
            pay.AddRequestData("vnp_Command", _vnPayConfig.Command);
            pay.AddRequestData("vnp_TmnCode", _vnPayConfig.TmnCode);
            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _vnPayConfig.CurrCode);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _vnPayConfig.Locale);
            pay.AddRequestData("vnp_OrderInfo", $"{model.OrderDescription}");
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", callbackUrl);
            pay.AddRequestData("vnp_ExpireDate", expiredTime.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl =
                pay.CreateRequestUrl(_vnPayConfig.BaseUrl, _vnPayConfig.HashSecret);

            return paymentUrl;
        }

        public PaymentResponse PaymentExecute(IQueryCollection collections)
        {
            var pay = new VNPay();
            var response = pay.GetFullResponseData(collections, _vnPayConfig.HashSecret);
            return response;
        }
    }
}
