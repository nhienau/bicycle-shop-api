using api.Configurations;
using api.Interfaces;
using api.Utilities.ZaloPay;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly ZaloPayConfig _zaloPayConfig;
        public PaymentController(IPaymentRepository paymentRepo, ZaloPayConfig zaloPayConfig)
        {
            _paymentRepo = paymentRepo;
            _zaloPayConfig = zaloPayConfig;
        }
        [HttpPost("zalopay/payment")]
        public async Task<IActionResult> GetZaloPayPaymentUrl([FromBody] String test)
        {
            Dictionary<string, object> result = await _paymentRepo.GetZaloPayPaymentUrl();
            return Ok(result);
        }

        [HttpPost("zalopay/callback")]
        public async Task<IActionResult> ZaloPayPaymentCallback([FromBody] dynamic data)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                var dataStr = Convert.ToString(data["data"]);
                var reqMac = Convert.ToString(data["mac"]);

                var mac = HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _zaloPayConfig.Key2, dataStr);

                if (!reqMac.Equals(mac))
                {
                    result["return_code"] = -1;
                    result["return_message"] = "mac not equal";
                }
                else
                {
                    Dictionary<string, object> dataJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataStr);
                    // Update
                    Console.WriteLine(dataJson);

                    result["return_code"] = 1;
                    result["return_message"] = "success";
                }
            }
            catch (Exception e)
            {
                result["return_code"] = 0;
                result["return_message"] = e.Message;
            }
            return Ok(result);
        }
    }
}
