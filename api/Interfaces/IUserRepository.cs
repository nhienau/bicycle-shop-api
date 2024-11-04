using api.Dtos.User;
using api.Utilities;

namespace api.Interfaces
{
    public interface IUserRepository
    {
        Task<PaginatedResponse<UserDTO>> GetAllAsync(UserQueryDTO query);
    }
}
