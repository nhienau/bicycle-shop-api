using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ICloudinaryRepository _cloudinaryRepo;
        public ProductImageRepository(ApplicationDBContext context, ICloudinaryRepository cloudinaryRepo)
        {
            _context = context;
            _cloudinaryRepo = cloudinaryRepo;
        }

        public async Task<ProductImage> CreateAsync(String url, int? productDetailId, int productId)
        {
            ProductImage productImage = new ProductImage { Url = url, ProductDetailId = productDetailId, ProductId = productId };
            await _context.AddAsync(productImage);
            await _context.SaveChangesAsync();
            return productImage;
        }

        public async Task DeleteAsync(List<int> id)
        {
            List<ProductImage> images = await _context.ProductImages.Where(e => id.Contains(e.Id)).ToListAsync();
            foreach (ProductImage image in images)
            {
                _context.ProductImages.Remove(image);
                await _cloudinaryRepo.DeleteImageAsync(image.Url);
            }
            await _context.SaveChangesAsync();
        }
    }
}
