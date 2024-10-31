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
        public async Task<IActionResult> UploadAsync([FromForm] List<IFormFile> files, [FromForm] List<int?> productDetailIds, [FromForm] int productId)
        {
            List<ProductImage> results = new List<ProductImage>();
            for (int i = 0; i < files.Count; i++) {
                IFormFile file = files[i];
                int? productDetailId = productDetailIds[i];

                String url = await _cloudinaryRepo.UploadImageAsync(file);
                ProductImage result = await _productImageRepo.CreateAsync(url, productDetailId, productId);
                results.Add(result);
            }
            return Ok();
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteImageRequestDto req)
        {
            List<int> id = req.Id;
            await _productImageRepo.DeleteAsync(id);
            return Ok();
        }
    }
}
