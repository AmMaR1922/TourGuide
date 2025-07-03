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

            var ReviewExists = await unitOfWork.Repository<TripReviews>().GetAll().FirstOrDefaultAsync(r => r.TripId == reviewDto.TripId && r.UserId == user.Id);

            if (ReviewExists != null)
                return APIResponse<string>.FailureResponse(500, null, "can't have more than one review");

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

        public async Task<APIResponse<string>> Update(ReviewDTORequest reviewDto, ApplicationUser user)
        {
            if (user is null)
                return APIResponse<string>.FailureResponse(401, null, "User not found");

            var trip = await unitOfWork.Repository<Trip>().GetByIdAsync(reviewDto.TripId);

            if (trip is null)
                return APIResponse<string>.FailureResponse(404, null, "Trip not found");

            var Review = await unitOfWork.Repository<TripReviews>().GetAll().FirstOrDefaultAsync(r => r.TripId == reviewDto.TripId && r.UserId == user.Id);

            if (Review == null)
                return APIResponse<string>.FailureResponse(404, null, "Review not found.");

            Review.Rating = reviewDto.Rating;
            Review.Comment = reviewDto.Comment;

            unitOfWork.Repository<TripReviews>().Update(Review);
            var result = await unitOfWork.CompleteAsync();

            if (!result)
                return APIResponse<string>.FailureResponse(500, null, "An Error occured while updating review");
            return APIResponse<string>.SuccessResponse(200, null, "Review Updated Successfully");
        }

        public async Task<APIResponse<string>> Delete(int TripId, ApplicationUser user)
        {
            if (user is null)
                return APIResponse<string>.FailureResponse(401, null, "User not found");

            var trip = await unitOfWork.Repository<Trip>().GetByIdAsync(TripId);

            if (trip is null)
                return APIResponse<string>.FailureResponse(404, null, "Trip not found");

            var Review = await unitOfWork.Repository<TripReviews>().GetAll().FirstOrDefaultAsync(r => r.TripId == TripId && r.UserId == user.Id);

            if (Review == null)
                return APIResponse<string>.FailureResponse(404, null, "Review not found.");

            unitOfWork.Repository<TripReviews>().Delete(Review);
            var result = await unitOfWork.CompleteAsync();

            if (!result)
                return APIResponse<string>.FailureResponse(500, null, "An Error occured while deleting review");
            return APIResponse<string>.SuccessResponse(200, null, "Review Deleted Successfully");
        }
    }
}
