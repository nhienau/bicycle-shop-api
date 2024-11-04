using api.Dtos.ProductImage;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Interfaces
{
    public interface IProductImageRepository
    {
        Task<ProductImage> CreateAsync(String url, int? productDetailId, int productId);

        Task DeleteAsync(List<int> id);
    }
}
