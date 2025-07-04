using ApplicationLayer.Contracts.Services;
using ApplicationLayer.DTOs.Trip;
using ApplicationLayer.DTOs.TripDtos;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TourGuide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TripController : ControllerBase
    {
        private readonly ITripServices tripServices;

        public TripController(ITripServices tripServices)
        {
            this.tripServices = tripServices;
        }

        [HttpGet("GetAllTrips")]
        public async Task<ActionResult<APIResponse<Pagination<TripDTOResponse>>>> GetAllTrips([FromQuery] TripSpecParams Params)
        {
            var isAdmin = User.IsInRole("Admin");
            var response = await tripServices.GetAll(Params, isAdmin);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetTripById/{Id}")]
        public async Task<ActionResult<APIResponse<TripToBeReturnedDTO>>> GetTripById(int Id)
        {
            var response = await tripServices.GetById(Id);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddTrip")]
        public async Task<ActionResult<APIResponse<string>>> AddTrip([FromForm] TripToBeAddedDTO tripDto)
        {
            var response = await tripServices.Add(tripDto);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateTrip/{Id}")]
        public async Task<ActionResult<APIResponse<string>>> UpdateTrip([FromForm] TripToBeUpdatedDTO tripDto, int Id)
        {
            var response = await tripServices.Update(Id, tripDto);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteTrip/{Id}")]
        public async Task<ActionResult<string>> DeleteTrip(int Id)
        {
            var response = await tripServices.Delete(Id);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddImagesToTrip/{TripId}")]
        public async Task<ActionResult<APIResponse<string>>> AddImagesToTrip(int TripId, [FromForm] List<IFormFile> images)
        {
            var response = await tripServices.AddImagesToTrip(TripId, images);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteImagesFromTrip/{TripId}")]
        public async Task<ActionResult<APIResponse<string>>> DeleteImagesFromTrip(int TripId, [FromForm] List<int> imageIds)
        {
            var response = await tripServices.DeleteImagesFromTrip(TripId, imageIds);
            return StatusCode(response.StatusCode, response);
        }
    }
}
