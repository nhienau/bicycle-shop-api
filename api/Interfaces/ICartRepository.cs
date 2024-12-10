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

        //Cart GetCartByUserId(int userId);
        void UpdateCart(Cart cart);

        //Task SyncCartAsync(int userId, IEnumerable<CartItem> cartItems);

        //Task<CartItem> GetCartItemAsync(int userId, int productId);
        Task<List<CartItemDTO>> GetCartByUserIdAsync(int userId);

         Task SyncCartAsync(int userId, List<CartItemDTO> clientCart);
         Task SaveChangesAsync();

        Task AddToCartAsync(int userId, CartItemDTO cartItem);
        Task RemoveFromCartAsync(int userId, int productDetailId);

        Task<Cart> GetCartItemByIdAsync(int cartItemId);

        Task<bool> UpdateCartItemQuantityAsync(int cartItemId, int newQuantity);
        Task<bool> UpdateProductDetailStockAsync(int productDetailId, int changeInStock);

    }
}
