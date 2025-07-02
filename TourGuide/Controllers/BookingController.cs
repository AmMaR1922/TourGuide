using ApplicationLayer.Contracts.Services;
using ApplicationLayer.DTOs.Booking;
using ApplicationLayer.Models;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TourGuide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            return Ok(response);
        }
    }
}
