using api.Dtos.ProductDetail;
using api.Models;

namespace api.Interfaces
{
    public interface IProductDetailRepository
    {
        Task<ProductDetail?> GetByIdAsync(int id);
        Task<ProductDetail> CreateAsync(ProductDetail productDetail);
        Task<ProductDetail?> UpdateAsync(int id, ProductDetailRequestDto productDetailDto);
        Task<ProductDetail?> DeleteAsync(int id);
    }
}
