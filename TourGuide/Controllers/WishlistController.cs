using ApplicationLayer.Contracts.Services;
using ApplicationLayer.DTOs.Wishlist;
using ApplicationLayer.Models;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TourGuide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistServices wishlistServices;
        private readonly UserManager<ApplicationUser> userManager;

        public WishlistController(IWishlistServices wishlistServices, UserManager<ApplicationUser> userManager)
        {
            this.wishlistServices = wishlistServices;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse<List<WishlistDTOResponse>>>> GetWishList()
        {
            var user = await userManager.GetUserAsync(User);

            var response = await wishlistServices.GetAll(user);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("AddToWishlist/{TripId}")]
        public async Task<ActionResult<APIResponse<string>>> AddToWishlist(int TripId)
        {
            var user = await userManager.GetUserAsync(User);

            var response = await wishlistServices.Add(user, TripId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete]
        public async Task<ActionResult<APIResponse<string>>> RemoveFromWishList(int TripId)
        {
            var user = await userManager.GetUserAsync(User);

            var response = await wishlistServices.Delete(user, TripId);
            return StatusCode(response.StatusCode, response);
        }

    }
}
