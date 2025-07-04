﻿using ApplicationLayer.Contracts.Services;
using ApplicationLayer.DTOs.Review;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TourGuide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewsServices reviewsServices;
        private readonly UserManager<ApplicationUser> userManager;

        public ReviewsController(IReviewsServices reviewsServices, UserManager<ApplicationUser> userManager)
        {
            this.reviewsServices = reviewsServices;
            this.userManager = userManager;
        }

        [HttpGet("GetReviews")]
        public async Task<ActionResult<APIResponse<List<TripReviews>>>> GetAllReviews(ReviewsSpecParams Params)
        {
            var response = await reviewsServices.GetAll(Params);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize]
        [HttpPost("AddReview")]
        public async Task<APIResponse<string>> AddReview([FromBody] ReviewDTORequest reviewDTORequest)
        {
            var user = await userManager.GetUserAsync(User);

            var response = await reviewsServices.Add(reviewDTORequest, user);
            return APIResponse<string>.SuccessResponse(200, null, "Review added successfully");
        }

        [Authorize]
        [HttpDelete("DeleteReview/{TripId}")]
        public async Task<ActionResult<APIResponse<string>>> DeleteReview(int TripId)
        {
            var user = await userManager.GetUserAsync(User);
            var response = await reviewsServices.Delete(TripId, user);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize]
        [HttpPut("UpdateReview")]
        public async Task<ActionResult<APIResponse<string>>> UpdateReview([FromBody] ReviewDTORequest reviewDTORequest)
        {
            var user = await userManager.GetUserAsync(User);
            var response = await reviewsServices.Update(reviewDTORequest, user);
            return StatusCode(response.StatusCode, response);
        }
    }
}
