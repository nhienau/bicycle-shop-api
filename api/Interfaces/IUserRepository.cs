using api.Dtos.Product;
using api.Dtos.User;
using api.Models;
using api.Utilities;

namespace api.Interfaces
{
    public interface IUserRepository
    {
        Task<PaginatedResponse<UserDTO>> GetAllAsync(UserQueryDTO query);

        Task<User> CreateAsync(User user);

        Task<User?> GetByIdAsync(int id);

        Task<User?> UpdateAsync(int id, UpdateUserRequestDTO userDTO);
        Task<User?> DeleteAsync(int id);
    }
}
