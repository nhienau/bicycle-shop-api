using api.Dtos.Product;
using api.Models;
using api.Utilities;

namespace api.Interfaces
{
    public interface IProductRepository
    {
        Task<PaginatedResponse<ProductDTO>> GetAllAsync(ProductQueryDTO query);
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product product);
        Task<Product?> UpdateAsync(int id, UpdateProductRequestDto productDto);
        Task<Product?> DeleteAsync(int id);
        Task<bool> ProductExists(int id);
    }
}
