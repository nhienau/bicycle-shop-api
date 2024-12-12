using api.Configurations;
using api.Data;
using api.Dtos.Order;
using api.Interfaces;
using api.Models;
using api.Utilities.ZaloPay;
using Newtonsoft.Json;

namespace api.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ZaloPayConfig _zaloPayConfig;
        private readonly IProductDetailRepository _productDetailRepository;

        public PaymentRepository(ZaloPayConfig zaloPayConfig, IProductDetailRepository productDetailRepository)
        {
            _zaloPayConfig = zaloPayConfig;
            _productDetailRepository = productDetailRepository;
        }
        public async Task<Dictionary<string, object>> GetZaloPayPaymentUrl(OrderPaymentRequest req)
        {
            long totalPrice = 0;
            foreach (var od in req.OrderDetail)
            {
                ProductDetail? pd = await _productDetailRepository.GetByIdAsync(od.ProductDetailId);
                if (pd == null)
                {
                    throw new Exception("Product detail not found");
                } else
                {
                    totalPrice += od.Quantity * pd.Price;
                    od.Price = pd.Price;
                }
            }

            req.TotalPrice = totalPrice;

            Random rnd = new Random();
            var embedData = new {
                redirecturl = _zaloPayConfig.RedirectUrl
            };
            var item = new[] { req };
            int appTransId = rnd.Next(1000000);
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("app_id", _zaloPayConfig.AppId);
            param.Add("app_trans_id", DateTime.Now.ToString("yyMMdd") + "_" + appTransId);
            param.Add("app_time", Utils.GetTimeStamp().ToString());
            param.Add("app_user", "app_user");
            param.Add("amount", totalPrice.ToString());
            param.Add("description", "Description #" + appTransId);
            param.Add("bank_code", "");
            param.Add("item", JsonConvert.SerializeObject(item));
            param.Add("embed_data", JsonConvert.SerializeObject(embedData));
            param.Add("callback_url", _zaloPayConfig.CallbackUrl);
            param.Add("title", "Title #" + appTransId);

            string data = _zaloPayConfig.AppId + "|" + param["app_trans_id"] + "|" + param["app_user"] + "|" + param["amount"] + "|"
                + param["app_time"] + "|" + param["embed_data"] + "|" + param["item"];
            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _zaloPayConfig.Key1, data));

            Dictionary<string, object> result = await HttpHelper.PostFormAsync(_zaloPayConfig.Endpoint, param);
            
            return result;
        }
    }
}
