using ApplicationLayer.Contracts.Repositories.CategoryRepoository;
using ApplicationLayer.DTOs.CategoryDto;
using ApplicationLayer.Models;
using ApplicationLayer.Models.SpecificationParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TourGuide.Controllers.CategoryController
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public ICategoryRepository CategoryRepository { get; }

        public CategoryController(ICategoryRepository categoryRepository)
        {
            CategoryRepository = categoryRepository;
        }

        [HttpPost("AddCategory")]

        public async Task<IActionResult> AddCategory([FromBody] AddCategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest("Category data cannot be null.");
            }
            var response = await CategoryRepository.AddCategory(categoryDto);
            if (!response.Succeeded)
            {
                return BadRequest(response.Errors);
            }
            return Ok(APIResponse<AddCategoryDto>.SuccessResponse(response.Data, "Category Added successfully."));
        }


        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories([FromQuery] CategorySpecParams Params)
        {
            var response = await CategoryRepository.GetAllCategories(Params);
            if (!response.Succeeded)
            {
                return BadRequest(response.Errors);
            }
            return Ok(APIResponse<List<CategoryDtoResponse>>.SuccessResponse(response.Data, "Categories retrieved successfully."));

        }

        [HttpGet("GetCategoryById/{Id}")]
        public async Task<IActionResult> GetCategoryById(int Id)
        {
            var response = await CategoryRepository.GetCategoryById(Id);
            if (!response.Succeeded)
            {
                return NotFound(response.Errors);
            }
            return Ok(APIResponse<CategoryDtoResponse>.SuccessResponse(response.Data, "Category retrieved successfully."));

        }

        [HttpPut("UpdateCategory/{Id}")]
        public async Task<IActionResult> UpdateCategory([FromBody] AddCategoryDto categoryDto, int Id)
        {
            if (categoryDto == null)
            {
                return BadRequest("Category data cannot be null.");
            }
            var response = await CategoryRepository.UpdateCategory(categoryDto, Id);
            if (!response.Succeeded)
            {
                return BadRequest(response.Errors);
            }
            return Ok(APIResponse<UpdateCategoryDto>.SuccessResponse(response.Data, "Category updated successfully."));

        }
        [HttpDelete("DeleteCategory/{Id}")]
        public async Task<IActionResult> DeleteCategory(int Id)
        {
            var response = await CategoryRepository.DeleteCategory(Id);
            if (!response.Succeeded)
            {
                return NotFound(response.Errors);
            }
             return Ok(APIResponse<CategoryDtoResponse>.SuccessResponse(response.Data, "Category deleted successfully."));
        }

    }
}