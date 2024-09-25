using api.Models;

namespace api.Interfaces
{
    public interface IProductDetailRepository
    {
        Task<ProductDetail?> GetByIdAsync(int id);
    }
}
