using ApplicationLayer.Contracts.Services;
using ApplicationLayer.DTOs.Trip;
using ApplicationLayer.DTOs.TripDtos;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
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
            var response = await tripServices.GetAll(Params);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetTripById/{Id}")]
        public async Task<ActionResult<APIResponse<TripToBeReturnedDTO>>> GetTripById(int Id)
        {
            var response = await tripServices.GetById(Id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("AddTrip")]
        public async Task<ActionResult<APIResponse<string>>> AddTrip([FromForm] TripToBeAddedDTO tripDto)
        {
            var response = await tripServices.Add(tripDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("UpdateTrip/{Id}")]
        public async Task<ActionResult<APIResponse<string>>> UpdateTrip([FromForm] TripToBeUpdatedDTO tripDto, int Id)
        {
            var response = await tripServices.Update(Id, tripDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("DeleteTrip/{Id}")]
        public async Task<ActionResult<string>> DeleteTrip(int Id)
        {
            var response = await tripServices.Delete(Id);
            return StatusCode(response.StatusCode, response);
        }

    }
}
