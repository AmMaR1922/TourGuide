using ApplicationLayer.Contracts.Services;
using ApplicationLayer.DTOs.CategoryDto;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TourGuide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices categoryServices;

        public CategoryController(ICategoryServices categoryServices)
        {
            this.categoryServices = categoryServices;
        }

        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<APIResponse<Pagination<CategoryDTOResponse>>>> GetAllCategories([FromQuery] SpecParams Params)
        {
            var response = await categoryServices.GetAll(Params);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetCategoryById/{Id}")]
        public async Task<ActionResult<APIResponse<CategoryDTOResponse>>> GetCategoryById(int Id)
        {
            var response = await categoryServices.GetById(Id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("AddCategory")]
        public async Task<ActionResult<APIResponse<string>>> AddCategory([FromBody] CategoryDTORequest categoryDto)
        {
            var response = await categoryServices.Add(categoryDto);
            return StatusCode(response.StatusCode,response);
        }

        [HttpPut("UpdateCategory/{Id}")]
        public async Task<ActionResult<APIResponse<string>>> UpdateCategory(int Id, [FromBody] CategoryDTORequest categoryDto)
        {
            var response = await categoryServices.Update(Id, categoryDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("DeleteCategory/{Id}")]
        public async Task<ActionResult<APIResponse<string>>> DeleteCategory(int Id)
        {
            var response = await categoryServices.Delete(Id);
            return StatusCode(response.StatusCode, response);
        }

    }
}