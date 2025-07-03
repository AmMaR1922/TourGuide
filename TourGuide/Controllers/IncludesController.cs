using ApplicationLayer.Contracts.Services;
using ApplicationLayer.DTOs.Includes;
using ApplicationLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TourGuide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncludesController : ControllerBase
    {
        private readonly IIncludesServices includesServices;

        public IncludesController(IIncludesServices includesServices)
        {
            this.includesServices = includesServices;
        }

        [HttpGet("GetAllIncludes")]
        public async Task<ActionResult<APIResponse<List<IncludesDTOResponse>>>> GetAllIncludes()
        {
            var response = await includesServices.GetAll();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetIncludeById/{id}")]
        public async Task<ActionResult<APIResponse<IncludesDTOResponse>>> GetIncludeById(int id)
        {
            var response = await includesServices.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("AddInclude")]
        public async Task<ActionResult<APIResponse<string>>> AddInclude([FromBody] IncludesDTORequest includesDto)
        {
            var response = await includesServices.Add(includesDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("UpdateInclude/{id}")]
        public async Task<ActionResult<APIResponse<string>>> UpdateInclude(int id, [FromBody] IncludesDTORequest includesDto)
        {
            var response = await includesServices.Update(id, includesDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("DeleteInclude/{id}")]
        public async Task<ActionResult<APIResponse<string>>> DeleteInclude(int id)
        {
            var response = await includesServices.Delete(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
