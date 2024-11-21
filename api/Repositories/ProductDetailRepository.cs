using api.Data;
using api.Dtos.ProductDetail;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class ProductDetailRepository : IProductDetailRepository
    {
        private readonly ApplicationDBContext _context;
        public ProductDetailRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<ProductDetail> CreateAsync(ProductDetail productDetail)
        {
            await _context.ProductDetails.AddAsync(productDetail);
            await _context.SaveChangesAsync();
            return productDetail;
        }

        public async Task<ProductDetail?> DeleteAsync(int id)
        {
            ProductDetail? productDetail = await _context.ProductDetails.FirstOrDefaultAsync(x => x.Id == id);
            if (productDetail == null || (productDetail != null && productDetail.Status == false))
            {
                return null;
            }

            ProductImage? productImage = await _context.ProductImages.FirstOrDefaultAsync(x => x.ProductDetailId == id);
            if (productImage != null)
            {
                productImage.ProductDetailId = null;
            }

            productDetail.Status = false;
            await _context.SaveChangesAsync();
            return productDetail;
        }

        public async Task<ProductDetail?> GetByIdAsync(int id)
        {
            return await _context.ProductDetails.FindAsync(id);
        }

        public async Task<ProductDetail?> UpdateAsync(int id, ProductDetailRequestDto productDetailDto)
        {
            ProductDetail? existingProductDetail = await _context.ProductDetails.FirstOrDefaultAsync(x => x.Id == id);
            if (existingProductDetail == null || (existingProductDetail != null && existingProductDetail.Status == false))
            {
                return null;
            }

            existingProductDetail.Size = productDetailDto.Size;
            existingProductDetail.Color = productDetailDto.Color;
            existingProductDetail.Price = productDetailDto.Price;
            existingProductDetail.Quantity = productDetailDto.Quantity;
            existingProductDetail.ProductId = productDetailDto.ProductId;

            await _context.SaveChangesAsync();

            return existingProductDetail;
        }
    }
}
