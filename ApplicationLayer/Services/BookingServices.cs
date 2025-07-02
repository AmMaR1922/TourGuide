using ApplicationLayer.Contracts.Services;
using ApplicationLayer.Contracts.UnitToWork;
using ApplicationLayer.DTOs;
using ApplicationLayer.DTOs.Booking;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using DomainLayer.Entities;
using DomainLayer.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services
{
    public class BookingServices : IBookingServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public BookingServices(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public async Task<APIResponse<string>> Add(BookingToBeAddedDTO BookingDto, ApplicationUser user)
        {
            if(user is null)
                return APIResponse<string>.FailureResponse(401, null, "Unauthorized access. User not found.");

            var booking = new Booking
            {
                Adults = BookingDto.Adults,
                Children = BookingDto.Children,
                TripDate = BookingDto.TripDate,
                Status = BookingStatus.Pending,
                TripId = BookingDto.TripId,
                UserId = user.Id
            };

            await unitOfWork.Repository<Booking>().AddAsync(booking);
            var result = await unitOfWork.CompleteAsync();

            if(!result)
                return APIResponse<string>.FailureResponse(500, null, "Failed to add booking. Please try again later.");
            return APIResponse<string>.SuccessResponse(201, null, "Booking added successfully.");
        }

        public async Task<APIResponse<string>> Delete(int Id, ApplicationUser user)
        {
            if(user is null)
                return APIResponse<string>.FailureResponse(401, null, "Unauthorized access. User not found.");

            var booking = await unitOfWork.Repository<Booking>().GetByIdAsync(Id);

            if(booking is null)
                return APIResponse<string>.FailureResponse(404, null, "Booking not found.");

            var userRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();

            if (booking.UserId != user.Id && userRole?.ToLower() != "admin")
                return APIResponse<string>.FailureResponse(403, null, "You do not have permission to delete this booking.");

            unitOfWork.Repository<Booking>().Delete(booking);
            var result = await unitOfWork.CompleteAsync();

            if(!result)
                return APIResponse<string>.FailureResponse(500, null, "Failed to delete booking. Please try again later.");
            return APIResponse<string>.SuccessResponse(200, null, "Booking deleted successfully.");
        }

        public Task<APIResponse<Pagination<BookingDTOResponse>>> GetAll(BookingSpecParams Params, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<APIResponse<BookingDTOResponse>> GetById(int Id, ApplicationUser user)
        {
            if (user is null)
                return APIResponse<BookingDTOResponse>.FailureResponse(401, null, "Unauthorized access. User not found.");

            var userRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();

            var booking = await unitOfWork.Repository<Booking>().GetByIdAsync(Id);

            if (booking is null)
                return APIResponse<BookingDTOResponse>.FailureResponse(404, null, "Booking not found.");

            if (booking.UserId != user.Id && userRole?.ToLower() != "admin")
                return APIResponse<BookingDTOResponse>.FailureResponse(403, null, "You do not have permission to get this booking.");

            var bookingResponse = new BookingDTOResponse
            {
                Id = booking.Id,
                Adults = booking.Adults,
                Children = booking.Children,
                TripDate = booking.TripDate,
                Status = booking.Status,
                TripId = booking.TripId,
                UserId = booking.UserId,
                TotalCost = booking.Trip.Price * (booking.Adults + ((double)booking.Children / 2.0)),
                CreatedAt = booking.CreatedAt
            };

            return APIResponse<BookingDTOResponse>.SuccessResponse(200, bookingResponse, "Booking retrieved successfully.");
        }

        public Task<APIResponse<string>> Update(int Id, BookingToBeUpdatedDTO BookingDto, ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}
