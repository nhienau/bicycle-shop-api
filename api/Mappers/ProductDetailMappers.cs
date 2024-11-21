using api.Dtos.ProductDetail;
using api.Models;

namespace api.Mappers
{
    public static class ProductDetailMappers
    {
        public static ProductDetailDto ToProductDetailDto(this ProductDetail productDetail)
        {
            ProductDetailDto dto = new ProductDetailDto
            {
                Id = productDetail.Id,
                Size = productDetail.Size,
                Color = productDetail.Color,
                Price = productDetail.Price,
                Quantity = productDetail.Quantity,
                Status = productDetail.Status,
                ProductId = productDetail.ProductId,
            };
            return dto;
        }

        public static ProductDetailDto ToProductDetailDto1(this ProductDetail productDetail)
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
                Product = productDetail.Product.ToProductDto()
                //Carts = productDetail.Carts.Select(pd => pd.ToCartDto()).ToList()
            };
        }

        public static ProductDetail ToProductDetailFromRequestDto(this ProductDetailRequestDto productDetailDto)
        {
            return new ProductDetail
            {
                Size = productDetailDto.Size,
                Color = productDetailDto.Color,
                Price = productDetailDto.Price,
                Quantity = productDetailDto.Quantity,
                ProductId = productDetailDto.ProductId,
                Status = true,
            };
        }
    }
}
