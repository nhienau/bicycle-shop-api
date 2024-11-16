using api.Dtos.Product;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryRepository _productCategoryRepo;

        public ProductCategoryController(IProductCategoryRepository productCategoryRepo)
        {
            _productCategoryRepo = productCategoryRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductCategoryQueryDTO query)
        {
            PaginatedResponse<ProductCategoryDTO> list = await _productCategoryRepo.GetAllAsync(query);
            return Ok(list);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            ProductCategory? productCategory = await _productCategoryRepo.GetByIdAsync(id);
            if (productCategory == null || (productCategory != null && !productCategory.Status))
            {
                return NotFound();
            }
            return Ok(productCategory.ToProductCategoryDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCategoryRequestDto productCategoryDto)
        {
            ProductCategory productCategory = productCategoryDto.ToProductCategoryFromCreateDto();

            await _productCategoryRepo.CreateAsync(productCategory);

            return CreatedAtAction(nameof(GetById), new { id = productCategory.Id }, productCategory.ToProductCategoryDto());
        }
       
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProductCategoryRequestDto productCategoryDto)
        {
            ProductCategory? productCategory = await _productCategoryRepo.UpdateAsync(id, productCategoryDto);
            if (productCategory == null) {
                return NotFound();
            }
            return Ok(productCategory.ToProductCategoryDto());
        }

        
        [HttpPut]
        [Route("delete/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            ProductCategory? productCategory = await _productCategoryRepo.DeleteAsync(id);

            if (productCategory == null)
            {
                return NotFound();
            }

            return Ok(productCategory.ToProductCategoryDto());
        }
    }
}
