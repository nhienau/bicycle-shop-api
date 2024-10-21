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
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepo;

        public ProductController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductQueryDTO query)
        {
            PaginatedResponse<ProductDTO> list = await _productRepo.GetAllAsync(query);
            return Ok(list);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            Product? product = await _productRepo.GetByIdAsync(id);
            if (product == null || (product != null && !product.Status))
            {
                return NotFound();
            }
            return Ok(product.ToProductDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequestDto productDto)
        {
            Product product = productDto.ToProductFromCreateDto();

            await _productRepo.CreateAsync(product);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product.ToProductDto());
        }

       
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProductRequestDto productDto)
        {
            Product? product = await _productRepo.UpdateAsync(id, productDto);
            if (product == null) {
                return NotFound();
            }
            return Ok(product.ToProductDto());
        }

        
        [HttpPut]
        [Route("delete/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Product? product = await _productRepo.DeleteAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product.ToProductDto());
        }
    }
}
