using api.Dtos.Product;
using api.Models;

namespace api.Mappers
{
    public static class ProductMappers
    {
        public static ProductDTO ToProductDto(this Product productModel)
        {
            ProductDTO dto = new ProductDTO
            {
                Id = productModel.Id,
                Name = productModel.Name,
                Description = productModel.Description,
                Status = productModel.Status,
                ProductDetails = productModel.ProductDetails.Select(pd => pd.ToProductDetailDto()).ToList(),
            };
            if (productModel.ProductCategory != null)
            {
                dto.ProductCategory = productModel.ProductCategory.ToProductCategoryDto();
            }
            return dto;
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
