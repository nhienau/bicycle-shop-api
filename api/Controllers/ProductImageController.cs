using api.Dtos.ProductImage;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly ICloudinaryRepository _cloudinaryRepo;
        private readonly IProductImageRepository _productImageRepo;

        public ProductImageController(ICloudinaryRepository cloudinaryRepo, IProductImageRepository productImageRepo)
        {
            _cloudinaryRepo = cloudinaryRepo;
            _productImageRepo = productImageRepo;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadAsync([FromForm] List<IFormFile> files, [FromForm] int productId)
        {
            List<ProductImage> results = new List<ProductImage>();
            for (int i = 0; i < files.Count; i++) {
                IFormFile file = files[i];
                int? productDetailId = null;

                String url = await _cloudinaryRepo.UploadImageAsync(file);
                ProductImage result = await _productImageRepo.CreateAsync(url, productDetailId, productId);
                results.Add(result);
            }
            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteImageRequestDto req)
        {
            List<int> id = req.Id;
            DeleteImageResponse response = await _productImageRepo.DeleteAsync(id);
            return Ok(response);
        }

        [HttpPut("updateProductDetail")]
        public async Task<IActionResult> UpdateProductDetail([FromBody] UpdateProductDetailImageRequest req)
        {
            ProductImage productImage = await _productImageRepo.UpdateProductDetail(req);
            return Ok(productImage.ToProductImageDto());
        }

        [HttpPut("detachProductDetail")]
        public async Task<IActionResult> DetachProductDetail([FromBody] DetachProductDetailImageRequest req)
        {
            ProductImage productImage = await _productImageRepo.DetachProductDetail(req);
            return Ok(productImage.ToProductImageDto());
        }
    }
}
