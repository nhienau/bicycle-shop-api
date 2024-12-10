namespace api.Dtos.User
{
    public class ChangePassRequestDTO
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
