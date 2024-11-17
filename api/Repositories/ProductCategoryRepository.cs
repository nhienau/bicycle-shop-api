using api.Data;
using api.Dtos.Product;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Utilities;
using Azure.Core;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly ApplicationDBContext _context;
        public ProductCategoryRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<ProductCategory> CreateAsync(ProductCategory productCategory)
        {
            await _context.ProductCategories.AddAsync(productCategory);
            await _context.SaveChangesAsync();
            return productCategory;
        }

        public async Task<ProductCategory?> DeleteAsync(int id)
        {
            ProductCategory? productCategory = await this.GetByIdAsync(id);

            if (productCategory == null)
            {
                return null;
            }

            await _context.ProductCategories.Where(rec => rec.Id == id).ExecuteDeleteAsync();

            return productCategory;
        }

        public async Task<PaginatedResponse<ProductCategoryDTO>> GetAllAsync(ProductCategoryQueryDTO query)
        {
            List<ProductCategory> productCategories = await _context.ProductCategories.ToListAsync();            

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                productCategories = productCategories.Where(p => p.Name.Contains(query.Name)).ToList();
            }

            int totalElements = productCategories.Count();

            int recordsSkipped = (query.PageNumber - 1) * query.PageSize;

            var result = productCategories.Skip(recordsSkipped).Take(query.PageSize).ToList();
            var resultDto = result.Select(p => p.ToProductCategoryDto()).ToList();

            return new PaginatedResponse<ProductCategoryDTO>(resultDto, query.PageNumber, query.PageSize, totalElements);
        }

        public async Task<ProductCategory?> GetByIdAsync(int id)
        {
            return await _context.ProductCategories.FindAsync(id);
        }

        public async Task<ProductCategory?> GetByIdIncludeProdcutAsync(int id)
        {
            return await _context.ProductCategories.Include(productCategory1 => productCategory1.Products).FirstOrDefaultAsync(productCategory => productCategory.Id == id);
        }

        public async Task<bool> IsExistProductInProductCategory(int id)
        {
            ProductCategory? productCategory = await GetByIdIncludeProdcutAsync(id);

            return productCategory == null ? false : productCategory.Products.Any();
        }

        public async Task<ProductCategory?> UpdateAsync(int id, UpdateProductCategoryRequestDto productCategoryDto)
        {
            ProductCategory? existingProductCategory = await this.GetByIdAsync(id);

            if (existingProductCategory == null)
            {
                return null;
            }            

            existingProductCategory.Name = productCategoryDto.Name ?? existingProductCategory.Name;
            existingProductCategory.Status = productCategoryDto.Status ?? existingProductCategory.Status;            

            await _context.SaveChangesAsync();

            return existingProductCategory;
        }
    }
}
