using api.Data;
using api.Dtos.Product;
using api.Dtos.User;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Utilities;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;

namespace api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;
        //private readonly IPasswordHasher<User> _passwordHasher;
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

        public async Task<User> CreateAsync(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public Task<User?> GetByIdAsync(int id)
        {
            return _context.Users.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> UpdateAsync(int id, UpdateUserRequestDTO userDTO)
        {
            User? existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (existingUser == null || (existingUser != null && existingUser.Status == false))
            {
                return null;
            }

            existingUser.Email = userDTO.Name;
            existingUser.Password = userDTO.Password;
            existingUser.Name = userDTO.Name;
            existingUser.Address = userDTO.Address;
            existingUser.PhoneNumber = userDTO.PhoneNumber;

            await _context.SaveChangesAsync();

            return existingUser;
        }

        public async Task<User?> DeleteAsync(int id)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null || (user != null && user.Status == false))
            {
                return null;
            }

            //_context.Products.Remove(product);
            user.Status = false;
            await _context.SaveChangesAsync();
            return user;
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.SingleOrDefault(u => u.Name == username);
        }

        public User GetUserByUserEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task RegisterUserAsync(User user)
        {
            //user.Password = _passwordHasher.HashPassword(user, user.Password); // Băm mật khẩu
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(user); // Thêm người dùng vào database
            await _context.SaveChangesAsync();
        }
    }
}
