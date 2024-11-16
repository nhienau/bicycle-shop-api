using api.Dtos.Product;
using api.Models;
using api.Utilities;

namespace api.Interfaces
{
    public interface IProductCategoryRepository
    {
        Task<PaginatedResponse<ProductCategoryDTO>> GetAllAsync(ProductCategoryQueryDTO query);
        Task<ProductCategory?> GetByIdAsync(int id);
        Task<ProductCategory> CreateAsync(ProductCategory productCategory);
        Task<ProductCategory?> UpdateAsync(int id, UpdateProductCategoryRequestDto productCategoryDto);
        Task<ProductCategory?> DeleteAsync(int id);
        Task<bool> ProductCategoryExists(int id);
    }
}
