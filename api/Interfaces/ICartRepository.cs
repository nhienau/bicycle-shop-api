using api.Dtos.Cart;
using api.Dtos.User;
using api.Models;
using api.Utilities;

namespace api.Interfaces
{
    public interface ICartRepository
    {
        //public Task<List<CartDTO>> GetAllAsync();
        //Task<Cart> CreateAsync(Cart cart);
        //Task<List<CartDTO>> GetAllCartsAsync(int? userId, string anonymousId);
        Task<PaginatedResponse<CartDTO>> GetAllAsync(CartQueryDTO query);

        Task<PaginatedResponse<CartDTO>> GetCartByUserIdAsync(int id, CartQueryDTO query);

    }
}
