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
                Name = userModel.Name,
                Address = userModel.Address,
                PhoneNumber = userModel.PhoneNumber,
            };
        }

        public static User ToUserFromCreateDTO(this CreateUserRequestDTO userDTO)
        {
            return new User
            {
                Email = userDTO.Email,
                Password = userDTO.Password,
                Name = userDTO.Name,
                Address = userDTO.Address,
                PhoneNumber = userDTO.PhoneNumber,
                Status = true,
            };
        }
    }
}
