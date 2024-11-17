using api.Dtos.Product;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
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
            try
            {
                PaginatedResponse<ProductCategoryDTO> list = await _productCategoryRepo.GetAllAsync(query);
                return Ok(ApiResponse<PaginatedResponse<ProductCategoryDTO>>.Success(list));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<NoContent>.Fail(StatusCodes.Status500InternalServerError, null, ex.Message));
            }            
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                ProductCategory? productCategory = await _productCategoryRepo.GetByIdAsync(id);
                if (productCategory == null)
                {
                    return NotFound(ApiResponse<NoContent>.Fail(StatusCodes.Status404NotFound));
                }
                return Ok(ApiResponse<ProductCategoryDTO>.Success(productCategory.ToProductCategoryDto()));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<NoContent>.Fail(StatusCodes.Status500InternalServerError,null,ex.Message));
            }
           
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCategoryRequestDto productCategoryDto)
        {
            try
            {
                ProductCategory productCategory = productCategoryDto.ToProductCategoryFromCreateDto();

                await _productCategoryRepo.CreateAsync(productCategory);

                return CreatedAtAction(nameof(GetById), new { id = productCategory.Id }, ApiResponse<ProductCategoryDTO>.Success(productCategory.ToProductCategoryDto(), StatusCodes.Status201Created));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<NoContent>.Fail(StatusCodes.Status500InternalServerError, null, ex.Message));
            }
        }
       
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProductCategoryRequestDto productCategoryDto)
        {
            try
            {
                ProductCategory? productCategory = await _productCategoryRepo.GetByIdAsync(id);

                if (productCategory == null)
                {
                    return NotFound(ApiResponse<NoContent>.Fail(StatusCodes.Status404NotFound));
                }

                await _productCategoryRepo.UpdateAsync(id, productCategoryDto);

                return Ok(ApiResponse<ProductCategoryDTO>.Success(productCategory.ToProductCategoryDto()));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<NoContent>.Fail(StatusCodes.Status500InternalServerError, null, ex.Message));
            }           
        }

        [HttpPatch]
        [Route("delete/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                bool isExistProductInProductCategory = await _productCategoryRepo.IsExistProductInProductCategory(id);

                if (!isExistProductInProductCategory)
                {
                    UpdateProductCategoryRequestDto updateProductCategoryDto = new UpdateProductCategoryRequestDto
                    {
                        Status = false
                    };

                    ProductCategory? productCategory = await _productCategoryRepo.UpdateAsync(id, updateProductCategoryDto);

                    if (productCategory == null)
                    {
                        return NotFound(ApiResponse<NoContent>.Fail(StatusCodes.Status404NotFound));
                    }

                    return Ok(ApiResponse<ProductCategoryDTO>.Success(productCategory.ToProductCategoryDto()));
                }
                else
                {
                    return BadRequest(ApiResponse<NoContent>.Fail(StatusCodes.Status400BadRequest, "Cannot delete Product Category - it still contains products !"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<NoContent>.Fail(StatusCodes.Status500InternalServerError, null, ex.Message));
            }            
        }
    }
}
