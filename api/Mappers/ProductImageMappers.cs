
using api.Dtos.ProductImage;
using api.Models;

namespace api.Mappers
{
    public static class ProductImageMappers
    {
        public static ProductImageDto ToProductImageDto(this ProductImage productImage)
        {
            return new ProductImageDto
            {
                Id = productImage.Id,
                Url = productImage.Url,
                ProductDetailId = productImage.ProductDetailId,
                ProductId = productImage.ProductId,
            };
        }
    }
}
