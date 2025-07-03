using ApplicationLayer.Contracts.Services;
using ApplicationLayer.Contracts.UnitToWork;
using ApplicationLayer.DTOs.Wishlist;
using ApplicationLayer.Models;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services
{
    public class WishlistServices : IWishlistServices
    {
        private readonly IUnitOfWork unitOfWork;

        public WishlistServices(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<APIResponse<string>> Add(ApplicationUser user, int TripId)
        {
            if (user is null)
                return APIResponse<string>.FailureResponse(400, null, "User not found.");

            var trip = await unitOfWork.Repository<Trip>().GetByIdAsync(TripId);

            if (trip is null)
                return APIResponse<string>.FailureResponse(404, null, "Trip not found.");

            var existingWishlist = await unitOfWork.Repository<Wishlist>().GetAll().FirstOrDefaultAsync(w => w.TripId == TripId && w.UserId == user.Id);  
            
            if (existingWishlist != null)
            {
                return APIResponse<string>.FailureResponse(400, null, "Trip already exists in wishlist.");
            }

            var wishlist = new Wishlist()
            {
                TripId = TripId,
                UserId = user.Id
            };

            await unitOfWork.Repository<Wishlist>().AddAsync(wishlist);
            var result = await unitOfWork.CompleteAsync();

            if (!result)
                return APIResponse<string>.FailureResponse(500, null, "Failed to add trip to wishlist.");
            return APIResponse<string>.SuccessResponse(200, null, "Trip added to wishlist successfully.");
        }

        public async Task<APIResponse<string>> Delete(ApplicationUser user, int TripId)
        {
            if (user is null)
                return APIResponse<string>.FailureResponse(400, null, "User not found.");

            var trip = await unitOfWork.Repository<Trip>().GetByIdAsync(TripId);

            if (trip is null)
                return APIResponse<string>.FailureResponse(404, null, "Trip not found.");

            var wishlist = await unitOfWork.Repository<Wishlist>().GetAll().FirstOrDefaultAsync(w => w.TripId == TripId && w.UserId == user.Id);

            if (wishlist is null)
                return APIResponse<string>.FailureResponse(404, null, "Trip not found in wishlist.");

            unitOfWork.Repository<Wishlist>().Delete(wishlist);
            var result = await unitOfWork.CompleteAsync();

            if (!result)
                return APIResponse<string>.FailureResponse(500, null, "Failed to remove trip from wishlist.");
            return APIResponse<string>.SuccessResponse(200, null, "Trip removed from wishlist successfully.");
        }

        public async Task<APIResponse<List<WishlistDTOResponse>>> GetAll(ApplicationUser user)
        {
            if(user is null)
                return APIResponse<List<WishlistDTOResponse>>.FailureResponse(400, null, "User not found.");

            var wishlists = await unitOfWork.Repository<Wishlist>().GetAll()
                .Select(w => new WishlistDTOResponse
                {
                    TripId = w.TripId,
                })
                .ToListAsync();

            return APIResponse<List<WishlistDTOResponse>>.SuccessResponse(200, wishlists, "Wishlist retrieved successfully.");
        }
    }
}
