using api.Dtos.ProductImage;
using api.Models;

namespace api.Interfaces
{
    public interface IProductImageRepository
    {
        Task<ProductImage> CreateAsync(String url, int? productDetailId, int productId);
        Task<DeleteImageResponse> DeleteAsync(List<int> id);
        Task<ProductImage> UpdateProductDetail(UpdateProductDetailImageRequest req);
        Task<ProductImage> DetachProductDetail(DetachProductDetailImageRequest req);
    }
}
