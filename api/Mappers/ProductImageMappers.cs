
using api.Dtos.ProductImage;
using api.Models;

namespace api.Mappers
{
    public static class ProductImageMappers
    {
        public static ProductImageDto ToProductImageDto(this ProductImage productImage)
        {
            ProductImageDto productImageDto = new ProductImageDto
            {
                Id = productImage.Id,
                Url = productImage.Url,
                ProductDetailId = productImage.ProductDetailId,
                ProductId = productImage.ProductId,
            };
            if (productImage.ProductDetail != null)
            {
                productImageDto.ProductDetail = productImage.ProductDetail.ToProductDetailDto();
            }
            return productImageDto;
        }
    }
}
