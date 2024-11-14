using api.Dtos.Product;
using api.Dtos.User;
using api.Models;
using api.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace api.Interfaces
{
    public interface IUserRepository
    {
        Task<PaginatedResponse<UserDTO>> GetAllAsync(UserQueryDTO query);

        Task<User> CreateAsync(User user);

        Task<User?> GetByIdAsync(int id);

        Task<User?> UpdateAsync(int id, UpdateUserRequestDTO userDTO);
        Task<User?> DeleteAsync(int id);

        User GetUserByUsername(string username);
        User GetUserByUserEmail(string email);

        Task<User> GetUserByIdAsync(int userId);
        Task<User> GetUserByEmailAsync(string email);
        Task RegisterUserAsync(User user);

        //Task BlacklistTokenAsync(string token);
        //Task SaveOtpAsync(int userId, string otp);
        //Task<bool> ValidateOtpAsync(int userId, string otp);

    }
}
