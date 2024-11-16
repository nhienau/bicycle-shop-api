using api.Data;
using api.Dtos.Product;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Utilities;
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
            ProductCategory? productCategory = await _context.ProductCategories.FirstOrDefaultAsync(x => x.Id == id);

            if (productCategory == null || (productCategory != null && productCategory.Status == false))
            {
                return null;
            }
            
            productCategory.Status = false;
            await _context.SaveChangesAsync();
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

        public Task<ProductCategory?> GetByIdAsync(int id)
        {
            return _context.ProductCategories.FirstOrDefaultAsync(i => i.Id == id);
        }

        public Task<bool> ProductCategoryExists(int id)
        {
            return _context.ProductCategories.AnyAsync(p => p.Id == id);
        }

        public async Task<ProductCategory?> UpdateAsync(int id, UpdateProductCategoryRequestDto productCategoryDto)
        {
            ProductCategory? existingProductCategory = await _context.ProductCategories.FirstOrDefaultAsync(x => x.Id == id);

            if (existingProductCategory == null || (existingProductCategory != null && existingProductCategory.Status == false))
            {
                return null;
            }

            existingProductCategory.Name = productCategoryDto.Name;
            existingProductCategory.Status = productCategoryDto.Status;            

            await _context.SaveChangesAsync();

            return existingProductCategory;
        }
    }
}
