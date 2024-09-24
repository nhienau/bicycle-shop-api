using api.Dtos.Product;
using api.Models;

namespace api.Mappers
{
    public static class ProductMappers
    {
        public static ProductDTO ToProductDto(this Product productModel)
        {
            return new ProductDTO
            {
                Id = productModel.Id,
                Name = productModel.Name,
                Description = productModel.Description,
                Status = productModel.Status,
            };
        }
        public static Product ToProductFromCreateDto(this CreateProductRequestDto productDto)
        {
            return new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                ProductCategoryId = productDto.ProductCategoryId,
                Status = true,
            };
        }
    }
}
