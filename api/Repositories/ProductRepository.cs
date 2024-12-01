using api.Data;
using api.Dtos.Product;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Utilities;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _context;
        public ProductRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> DeleteAsync(int id)
        {
            Product? product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product == null || (product != null && product.Status == false))
            {
                return null;
            }

            //_context.Products.Remove(product);
            product.Status = false;
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<PaginatedResponse<ProductDTO>> GetAllAsync(ProductQueryDTO query)
        {
            IQueryable<Product> products = _context.Products
                .Where(p => p.Status == true)
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductDetails.Where(pd => pd.Status == true))
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                products = products.Where(p => p.Name.Contains(query.Name));
            }

            int totalElements = await products.CountAsync();

            int recordsSkipped = (query.PageNumber - 1) * query.PageSize;

            var result = await products.Skip(recordsSkipped).Take(query.PageSize).ToListAsync();
            var resultDto = result.Select(p => p.ToProductDto()).ToList();

            return new PaginatedResponse<ProductDTO>(resultDto, query.PageNumber, query.PageSize, totalElements);
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            return _context.Products
                .Include(p => p.ProductDetails.Where(pd => pd.Status == true))
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductImages)
                    .ThenInclude(pi => pi.ProductDetail)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public Task<bool> ProductExists(int id)
        {
            return _context.Products.AnyAsync(p => p.Id == id);
        }

        public async Task<Product?> UpdateAsync(int id, UpdateProductRequestDto productDto)
        {
            Product? existingProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (existingProduct == null || (existingProduct != null && existingProduct.Status == false))
            {
                return null;
            }

            existingProduct.Name = productDto.Name;
            existingProduct.Description = productDto.Description;
            existingProduct.ProductCategoryId = productDto.ProductCategoryId;

            await _context.SaveChangesAsync();

            return existingProduct;
        }
    }
}
