using System.ComponentModel.DataAnnotations;

namespace api.Dtos.User
{
    public class CreateUserRequestDTO
    {
        
            [Required]
            public string Email { get; set; } = string.Empty;
            [Required]
            public string Password { get; set; } = string.Empty;
            public string Name { get; set; }
            public string Address { get; set; }
            public string PhoneNumber { get; set; }
    }
}
