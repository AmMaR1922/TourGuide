using ApplicationLayer.Contracts.Services;
using ApplicationLayer.DTOs.Booking;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TourGuide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingServices bookingServices;
        private readonly UserManager<ApplicationUser> userManager;

        public BookingController(IBookingServices bookingServices, UserManager<ApplicationUser> userManager)
        {
            this.bookingServices = bookingServices;
            this.userManager = userManager;
        }

        [HttpPost("AddBooking")]
        public async Task<ActionResult<APIResponse<string>>> AddBooking([FromBody] BookingToBeAddedDTO bookingDto)
        {
            var user = await userManager.GetUserAsync(User);

            var response = await bookingServices.Add(bookingDto, user);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("DeleteBooking/{id}")]
        public async Task<ActionResult<APIResponse<string>>> DeleteBooking(int id)
        {
            var user = await userManager.GetUserAsync(User);

            var response = await bookingServices.Delete(id, user);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetBookingById/{id}")]
        public async Task<ActionResult<APIResponse<BookingDTOResponse>>> GetBookingById(int id)
        {
            var user = await userManager.GetUserAsync(User);

            var response = await bookingServices.GetById(id, user);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetAllBookings")]
        public async Task<ActionResult<APIResponse<Pagination<BookingDTOResponse>>>> GetAllBookings([FromQuery] BookingSpecParams bookingSpecParams)
        {
            var user = await userManager.GetUserAsync(User);

            var response = await bookingServices.GetAll(bookingSpecParams, user);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("UpdateBooking/{id}")]
        public async Task<ActionResult<APIResponse<string>>> UpdateBooking(int id, [FromBody] BookingToBeUpdatedDTO bookingDto)
        {
            var user = await userManager.GetUserAsync(User);

            var response = await bookingServices.Update(id, bookingDto, user);
            return StatusCode(response.StatusCode, response);
        }
    }
}
