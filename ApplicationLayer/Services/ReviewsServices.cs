using ApplicationLayer.Contracts.Services;
using ApplicationLayer.Contracts.UnitToWork;
using ApplicationLayer.DTOs.Review;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using ApplicationLayer.Specifications.ReviewSpecifications;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services
{
    public class ReviewsServices : IReviewsServices
    {
        private readonly IUnitOfWork unitOfWork;

        public ReviewsServices(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<string>> Add(ReviewDTORequest reviewDto, ApplicationUser user)
        {
            if(user is null)
                return APIResponse<string>.FailureResponse(401, null, "User not found");

            var trip = await unitOfWork.Repository<Trip>().GetByIdAsync(reviewDto.TripId);

            if (trip is null)
                return APIResponse<string>.FailureResponse(404, null, "Trip not found");

            var review = new TripReviews()
            {
                Comment = reviewDto.Comment,
                Rating = reviewDto.Rating,
                TripId = reviewDto.TripId,
                UserId = user.Id,
            };

            await unitOfWork.Repository<TripReviews>().AddAsync(review);
            var result = await unitOfWork.CompleteAsync();

            if (!result)
                return APIResponse<string>.FailureResponse(500, null, "Failed to add review");
            return APIResponse<string>.SuccessResponse(200, null, "Review added successfully");
        }

        public async Task<APIResponse<Pagination<ReviewDTOResponse>>> GetAll(ReviewsSpecParams Params)
        {
            var Specs = new GetAllReviewsSpecs(Params);
            var reviews = await unitOfWork.Repository<TripReviews>().GetAllWithSpecification(Specs)
                .Select(r => new ReviewDTOResponse
                {
                    Comment = r.Comment,
                    Rating = r.Rating,
                    UserId = r.UserId,
                    CreatedAt = r.CreatedAt,
                })
                .ToListAsync();

            var CountSpecs = new CountAllReviewsSpecs(Params);
            var Count = await unitOfWork.Repository<TripReviews>().GetCountWithSpecs(CountSpecs);

            var Pagination = new Pagination<ReviewDTOResponse>(Params.PageNumber, Params.PageSize, Count, reviews);

            return APIResponse<Pagination<ReviewDTOResponse>>.SuccessResponse(200, Pagination, "Reviews retrieved successfully");
        }
    }
}
