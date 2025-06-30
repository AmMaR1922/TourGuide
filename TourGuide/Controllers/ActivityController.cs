using ApplicationLayer.Contracts.Services;
using ApplicationLayer.DTOs.Activity;
using ApplicationLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TourGuide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityServices activityServices;

        public ActivityController(IActivityServices activityServices)
        {
            this.activityServices = activityServices;
        }

        [HttpGet("GetAllActivities")]
        public async Task<ActionResult<APIResponse<List<ActivityDTOResponse>>>> GetAllActivities()
        {
            var response = await activityServices.GetAll();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetActivityById/{id}")]
        public async Task<ActionResult<APIResponse<ActivityDTOResponse>>> GetActivityById(int id)
        {
            var response = await activityServices.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("AddActivity")]
        public async Task<ActionResult<APIResponse<string>>> AddActivity([FromBody] ActivityToBeAddedDTO activityDto)
        {
            var response = await activityServices.Add(activityDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("UpdateActivity/{id}")]
        public async Task<ActionResult<APIResponse<string>>> UpdateActivity(int id, [FromBody] ActivityToBeUpdatedDTO activityDto)
        {
            var response = await activityServices.Update(id, activityDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("DeleteActivity/{id}")]
        public async Task<ActionResult<APIResponse<string>>> DeleteActivity(int id)
        {
            var response = await activityServices.Delete(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
