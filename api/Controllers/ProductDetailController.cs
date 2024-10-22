using api.Dtos.ProductDetail;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailController : ControllerBase
    {
        private readonly IProductDetailRepository _productDetailRepo;

        public ProductDetailController(IProductDetailRepository productDetailRepo)
        {
            _productDetailRepo = productDetailRepo;
        }
        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            ProductDetail? productDetail = await _productDetailRepo.GetByIdAsync(id);
            if (productDetail == null || (productDetail != null && !productDetail.Status))
            {
                return NotFound();
            }
            return Ok(productDetail.ToProductDetailDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDetailRequestDto productDetailDto)
        {
            ProductDetail productDetail = productDetailDto.ToProductDetailFromRequestDto();
            await _productDetailRepo.CreateAsync(productDetail);
            return CreatedAtAction(nameof(GetById), new { id = productDetail.Id }, productDetail.ToProductDetailDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ProductDetailRequestDto productDetailDto)
        {
            ProductDetail? productDetail = await _productDetailRepo.UpdateAsync(id, productDetailDto);
            if (productDetail == null)
            {
                return NotFound();
            }
            return Ok(productDetail.ToProductDetailDto());
        }

        [HttpPut]
        [Route("delete/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            ProductDetail? productDetail = await _productDetailRepo.DeleteAsync(id);

            if (productDetail == null)
            {
                return NotFound();
            }

            return Ok(productDetail.ToProductDetailDto());
        }
    }
}
