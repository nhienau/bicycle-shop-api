using api.Dtos.User;
using api.Models;

namespace api.Mappers
{
    public static class UserMapper
    {
        public static UserDTO ToUserDto(this User userModel)
        {
            return new UserDTO
            {
                Id = (int)userModel.Id,
                Email = userModel.Email,
                Password= userModel.Password,
                Name = userModel.Name,
                Address = userModel.Address,
                PhoneNumber = userModel.PhoneNumber,
            };
        }
    }
}
