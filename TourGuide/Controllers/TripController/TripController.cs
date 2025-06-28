using ApplicationLayer.Contracts.Repositories;
using ApplicationLayer.DTOs.TripDtos;
using ApplicationLayer.Models;
using ApplicationLayer.Models.SpecificationParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TourGuide.Controllers.TripController
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        public ITripRepository TripRepository { get; }
        public TripController(ITripRepository tripRepository)
        {
            TripRepository = tripRepository;
        }


        [HttpPost("AddTrip")]
        public async Task<IActionResult> AddTrip([FromBody]  AddTripDto tripDto)
        {
            if (tripDto == null)
            {
                return BadRequest("Trip data cannot be null.");
            }
            var response = await TripRepository.AddTrip(tripDto);
            if (!response.Succeeded)
            {
                return BadRequest(response.Errors);
            }
            if (tripDto.Languages.Any(string.IsNullOrWhiteSpace))
            {
                return BadRequest("Language codes cannot be null or empty.");
            }
            return Ok(APIResponse<AddTripDto>.SuccessResponse(response.Data, "Trip Added successfully."));
        }

        [HttpGet("GetAllTrips")]
        public async Task<IActionResult> GetAllTrips([FromQuery] TripSpecParams Params)
        {
            var response = await TripRepository.GetAllTrips(Params);
            if (!response.Succeeded)
            {
                return BadRequest(response.Errors);
            }
            return Ok(APIResponse<List<TripDtoResponse>>.SuccessResponse(response.Data, "Trips retrieved successfully."));
        }

        [HttpGet("GetTripById/{Id}")]
        public async Task<IActionResult> GetTripById(int Id)
        {
            var response = await TripRepository.GetTripById(Id);
            if (!response.Succeeded)
            {
                return NotFound(response.Errors);
            }
            return Ok(APIResponse<TripDtoResponse>.SuccessResponse(response.Data, "Trip retrieved successfully."));
        }

        [HttpPut("UpdateTrip/{Id}")]
        public async Task<IActionResult> UpdateTrip([FromBody] UpdateTripDto tripDto, int Id)
        {
            if (tripDto == null)
            {
                return BadRequest("Trip data cannot be null.");
            }
            var response = await TripRepository.UpdateTrip(tripDto, Id);
            if (!response.Succeeded)
            {
                return BadRequest(response.Errors);
            }
            return Ok(APIResponse<UpdateTripDto>.SuccessResponse(response.Data, "Trip updated successfully."));
        }

        [HttpDelete("DeleteTrip/{Id}")]
        public async Task<IActionResult> DeleteTrip(int Id)
        {
            var response = await TripRepository.DeleteTrip(Id);
            if (!response.Succeeded)
            {
                return NotFound(response.Errors);
            }
            return Ok(APIResponse<TripDtoResponse>.SuccessResponse(response.Data, "Trip deleted successfully."));
        }

        [HttpGet("GetTripsByCategoryId/{categoryId}")]
        public async Task<IActionResult> GetTripsByCategoryId(int categoryId)
        {
            var response = await TripRepository.GetTripsByCategoryId(categoryId);
            if (!response.Succeeded)
            {
                return NotFound(response.Errors);
            }
            return Ok(APIResponse<List<TripDtoResponse>>.SuccessResponse(response.Data, "Trips by category ID retrieved successfully."));
        }

        [HttpGet("GetTripsByCategoryName/{categoryName}")]
        public async Task<IActionResult> GetTripsByCategoryName(string categoryName)
        {
            var response = await TripRepository.GetTripsByCategoryName(categoryName);
            if (!response.Succeeded)
            {
                return NotFound(response.Errors);
            }
            return Ok(APIResponse<List<TripDtoResponse>>.SuccessResponse(response.Data, "Trips by category name retrieved successfully."));
        }
        [HttpGet("GetTripsByRating/{rating}")]
        public async Task<IActionResult> GetTripsByRating(double rating)
        {
            var response = await TripRepository.GetTripsByRating(rating);
            if (!response.Succeeded)
            {
                return NotFound(response.Errors);
            }
            return Ok(APIResponse<List<TripDtoResponse>>.SuccessResponse(response.Data, "Trips by rating retrieved successfully."));
        }
   
        }
}
