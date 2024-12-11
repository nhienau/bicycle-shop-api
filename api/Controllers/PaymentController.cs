using api.Configurations;
using api.Dtos.Order;
using api.Dtos.OrderStatus;
using api.Dtos.VNPay;
using api.Interfaces;
using api.Mappers;
using api.Models;
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
        private readonly IOrderRepository _orderRepo;
        private readonly ZaloPayConfig _zaloPayConfig;
        private readonly IVNPayService _vnPayService;
        public PaymentController(IPaymentRepository paymentRepo, IOrderRepository orderRepo, ZaloPayConfig zaloPayConfig, IVNPayService vnPayService)
        {
            _paymentRepo = paymentRepo;
            _orderRepo = orderRepo;
            _zaloPayConfig = zaloPayConfig;
            _vnPayService = vnPayService;
        }
        [HttpPost("zalopay/payment")]
        public async Task<IActionResult> GetZaloPayPaymentUrl([FromBody] OrderPaymentRequest request)
        {
            Dictionary<string, object> result = await _paymentRepo.GetZaloPayPaymentUrl(request);
            return Ok(result);
        }

        [HttpPost("zalopay/callback")]
        public async Task<IActionResult> ZaloPayPaymentCallback([FromBody] dynamic data)
        {
            System.Diagnostics.Debug.WriteLine("test");
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

                    OrderPaymentRequest orderInfo = JsonConvert.DeserializeObject<OrderPaymentRequest>(dataStr);
                    await _orderRepo.CreateOrderAsync(orderInfo);

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

        [HttpPost("vnpay/payment")]
        public IActionResult CreatePaymentUrlVnpay([FromBody] PaymentInfo model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
            return Ok(url);
        }

        [HttpGet("vnpay/callback")]
        public async Task<IActionResult> VnpayPaymentCallback()
        {
            IQueryCollection collections = Request.Query;
            PaymentResponse response = _vnPayService.PaymentExecute(collections);
            string orderIdStr = response.OrderDescription;
            int orderId = int.Parse(orderIdStr);
            PaymentCallbackResponse res = new PaymentCallbackResponse
            {
                Success = false,
                Order = null,
            };
            if (response.Success)
            {
                UpdateOrderStatusRequest req = new UpdateOrderStatusRequest
                {
                    OrderId = orderId,
                    StatusName = "Chờ xác nhận"
                };
                Order? o = await _orderRepo.UpdateOrderStatus(req);
                res.Success = o != null;
                res.Order = o?.ToOrderDTO();
            }
            return Ok(res);
        }
    }
}
