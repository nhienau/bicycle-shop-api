namespace api.Models
{
    public class VerifyOtpRequest
    {
        public string OtpToken { get; set; }   // Token mã hóa được gửi tới người dùng
        public string EnteredOtp { get; set; } // OTP người dùng nhập
    }
}
