using api.Models;

namespace api.Interfaces
{
    public interface IProductImageRepository
    {
        Task<ProductImage> CreateAsync(String url, int? productDetailId, int productId);
        Task DeleteAsync(List<int> id);
    }
}
