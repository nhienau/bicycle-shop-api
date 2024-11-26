using api.Dtos.ProductCategory;
﻿using api.Dtos.Product;
using api.Models;

namespace api.Mappers
{
    public static class ProductCategoryMappers
    {
        public static ProductCategoryDTO ToProductCategoryDto(this ProductCategory productCategoryModel)
        {
            return new ProductCategoryDTO
            {
                Id = productCategoryModel.Id,
                Name = productCategoryModel.Name,                
                Status = productCategoryModel.Status               
            };
        }
        
        public static ProductCategory ToProductCategoryFromCreateDto(this CreateProductCategoryRequestDto productCategoryDto)
        {
            return new ProductCategory
            {
                Name = productCategoryDto.Name,
                Status = productCategoryDto.Status,
            };
        }
    }
}
