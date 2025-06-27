using ApplicationLayer.Contracts.Services;
using ApplicationLayer.DTOs.CategoryDto;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TourGuide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices categoryServices;

        public CategoryController(ICategoryServices categoryServices)
        {
            this.categoryServices = categoryServices;
        }

        [HttpPost("AddCategory")]
        public async Task<ActionResult<APIResponse<string>>> AddCategory([FromBody] CategoryToBeAddedDTO categoryDto)
        {
            var response = await categoryServices.Add(categoryDto);
            return Ok(response);
        }


        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<APIResponse<Pagination<CategoryDTOResponse>>>> GetAllCategories([FromQuery] SpecParams Params)
        {
            var response = await categoryServices.GetAll(Params);
            return Ok(response);
        }

        [HttpGet("GetCategoryById/{Id}")]
        public async Task<ActionResult<APIResponse<CategoryDTOResponse>>> GetCategoryById(int Id)
        {
            var response = await categoryServices.GetById(Id);
            return Ok(response);
        }

        [HttpPut("UpdateCategory/{Id}")]
        public async Task<ActionResult<APIResponse<string>>> UpdateCategory(int Id, [FromBody] CategoryToBeUpdatedDTO categoryDto)
        {
            var response = await categoryServices.Update(Id, categoryDto);
            return Ok(response);
        }

        [HttpDelete("DeleteCategory/{Id}")]
        public async Task<ActionResult<APIResponse<string>>> DeleteCategory(int Id)
        {
            var response = await categoryServices.Delete(Id);
            return Ok(response);
        }

    }
}