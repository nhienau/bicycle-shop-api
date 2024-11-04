using api.Data;
using api.Dtos.User;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Utilities;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;
        public UserRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<UserDTO>> GetAllAsync(UserQueryDTO query)
        {
            IQueryable<User> users = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                users = users.Where(p => p.Name.Contains(query.Name));
            }

            int totalElements = await users.CountAsync();

            int recordsSkipped = (query.PageNumber - 1) * query.PageSize;

            var result = await users.Skip(recordsSkipped).Take(query.PageSize).ToListAsync();
            var resultDto = result.Select(p => p.ToUserDto()).ToList();

            return new PaginatedResponse<UserDTO>(resultDto, query.PageNumber, query.PageSize, totalElements);
        }
    }
}
