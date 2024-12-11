using System.ComponentModel.DataAnnotations;

namespace api.Dtos.User
{
    public class UserQueryDTO
    {
        public string? Name { get; set; } = string.Empty;
        [RegularExpression(@"^0\d{9,11}$", ErrorMessage = "Phone number must start with 0 and be 10-12 digits long")]
        public string? PhoneNumber { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
