using api.Dtos.ProductCategory;
using api.Models;

namespace api.Mappers
{
    public static class ProductCategoryMappers
    {
        public static ProductCategoryDto ToProductCategoryDto(this ProductCategory productCategory)
        {
            return new ProductCategoryDto
            {
                Id = productCategory.Id,
                Name = productCategory.Name,
                Status = productCategory.Status,
            };
        }
    }
}
