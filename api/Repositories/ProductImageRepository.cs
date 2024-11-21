using api.Data;
using api.Dtos.ProductImage;
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

        public async Task<DeleteImageResponse> DeleteAsync(List<int> id)
        {
            List<ProductImage> images = await _context.ProductImages.Where(e => id.Contains(e.Id)).ToListAsync();
            List<int> successfullyDeleted = [];
            foreach (ProductImage image in images)
            {
                _context.ProductImages.Remove(image);
                await _cloudinaryRepo.DeleteImageAsync(image.Url);
                successfullyDeleted.Add(image.Id);
            }
            await _context.SaveChangesAsync();
            DeleteImageResponse response = new DeleteImageResponse();
            response.Id = successfullyDeleted;
            return response;
        }

        public async Task<ProductImage> DetachProductDetail(DetachProductDetailImageRequest req)
        {
            int productImageId = req.ProductImageId;

            ProductImage? productImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.Id == productImageId);
            if (productImage == null)
            {
                throw new Exception("Image not found");
            }

            productImage.ProductDetailId = null;
            await _context.SaveChangesAsync();
            return productImage;
        }

        public async Task<ProductImage> UpdateProductDetail(UpdateProductDetailImageRequest req)
        {
            int currentImageId = req.CurrentImageId;
            int newImageId = req.NewImageId;
            int productDetailId = req.ProductDetailId;

            ProductImage? currentProductImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.Id == currentImageId);
            ProductImage? newProductImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.Id == newImageId);

            if (newProductImage == null)
            {
                throw new Exception("The image to be replaced not found");
            }

            if (newProductImage.ProductDetailId != null)
            {
                throw new Exception("The image to be replaced has been attached to another product detail.");
            }

            if (currentProductImage != null)
            {
                currentProductImage.ProductDetailId = null;
            }

            newProductImage.ProductDetailId = productDetailId;
            await _context.SaveChangesAsync();
            return newProductImage;
        }


    }
}
