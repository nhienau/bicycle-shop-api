using api.Dtos.ProductDetail;
using api.Models;

namespace api.Mappers
{
    public static class ProductDetailMappers
    {
        public static ProductDetailDto ToProductDetailDto(this ProductDetail productDetail)
        {
            return new ProductDetailDto
            {
                Id = productDetail.Id,
                Size = productDetail.Size,
                Color = productDetail.Color,
                Price = productDetail.Price,
                Quantity = productDetail.Quantity,
                Status = productDetail.Status,
                ProductId = productDetail.ProductId,
            };
        }
    }
}
