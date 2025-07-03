using ApplicationLayer.Contracts.Services;
using ApplicationLayer.Contracts.UnitToWork;
using ApplicationLayer.DTOs.Booking;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using ApplicationLayer.Specifications.BookingsSpecifications;
using DomainLayer.Entities;
using DomainLayer.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

            var userRole = (await userManager.GetRolesAsync(user))?.FirstOrDefault()?.ToLower();

            if (booking.UserId != user.Id && userRole != "admin")
                return APIResponse<string>.FailureResponse(403, null, "You do not have permission to delete this booking.");

            unitOfWork.Repository<Booking>().Delete(booking);
            var result = await unitOfWork.CompleteAsync();

            if(!result)
                return APIResponse<string>.FailureResponse(500, null, "Failed to delete booking. Please try again later.");
            return APIResponse<string>.SuccessResponse(200, null, "Booking deleted successfully.");
        }

        public async Task<APIResponse<Pagination<BookingDTOResponse>>> GetAll(BookingSpecParams Params, ApplicationUser user)
        {
            if (user is null)
                return APIResponse<Pagination<BookingDTOResponse>>.FailureResponse(401, null, "Unauthorized Access.");

            var userRole = (await userManager.GetRolesAsync(user))?.FirstOrDefault()?.ToLower();

            var Specs = new GetAllBookingsSpecs(Params, user.Id, userRole);
            var bookings = await unitOfWork.Repository<Booking>().GetAllWithSpecification(Specs)
                .Select(b => new BookingDTOResponse
                {
                    Id = b.Id,
                    Adults = b.Adults,
                    Children = b.Children,
                    TripDate = b.TripDate,
                    Status = b.Status,
                    TripId = b.TripId,
                    UserId = b.UserId,
                    TotalCost = b.Trip.Price * (b.Adults + ((decimal)b.Children / 2)),
                    CreatedAt = b.CreatedAt
                })
                .ToListAsync();
            
            var CountSpecs = new CountAllBookingsSpecs(Params, user.Id, userRole);
            int Count = await unitOfWork.Repository<Booking>().GetCountWithSpecs(CountSpecs);

            var Pagination = new Pagination<BookingDTOResponse>(Params.PageNumber, Params.PageSize, Count, bookings);

            return APIResponse<Pagination<BookingDTOResponse>>.SuccessResponse(200, Pagination, "Bookings retrieved successfully.");

        }

        public async Task<APIResponse<BookingDTOResponse>> GetById(int Id, ApplicationUser user)
        {
            if (user is null)
                return APIResponse<BookingDTOResponse>.FailureResponse(401, null, "Unauthorized access. User not found.");

            var userRole = (await userManager.GetRolesAsync(user))?.FirstOrDefault()?.ToLower();

            var booking = await unitOfWork.Repository<Booking>().GetByIdAsync(Id);

            if (booking is null)
                return APIResponse<BookingDTOResponse>.FailureResponse(404, null, "Booking not found.");

            if (booking.UserId != user.Id && userRole != "admin")
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
                TotalCost = booking.Trip.Price * (booking.Adults + ((decimal)booking.Children / 2)),
                CreatedAt = booking.CreatedAt
            };

            return APIResponse<BookingDTOResponse>.SuccessResponse(200, bookingResponse, "Booking retrieved successfully.");
        }

        public async Task<APIResponse<string>> Update(int Id, BookingToBeUpdatedDTO BookingDto, ApplicationUser user)
        {
            if (user is null)
                return APIResponse<string>.FailureResponse(401, null, "Unauthorized access. User not found.");

            var userRole = (await userManager.GetRolesAsync(user))?.FirstOrDefault()?.ToLower();
            
            if(userRole != "admin")
                return APIResponse<string>.FailureResponse(403, null, "You do not have permission to update bookings.");

            var booking = await unitOfWork.Repository<Booking>().GetByIdAsync(Id);

            if (booking is null)
                return APIResponse<string>.FailureResponse(404, null, "Booking not found.");

            if(booking.UserId != user.Id && userRole != "admin")
                return APIResponse<string>.FailureResponse(403, null, "You do not have permission to update this booking.");

            if(userRole == "normaluser" && (DateTime.UtcNow - booking.CreatedAt) > TimeSpan.FromMinutes(30))
                return APIResponse<string>.FailureResponse(403, null, "You cannot update a booking after 30 minutes of its creation.");

            booking.Adults = BookingDto.Adults;
            booking.Children = BookingDto.Children;
            booking.TripDate = BookingDto.TripDate;
            booking.Status = userRole == "admin" ? BookingDto.Status : booking.Status;

            unitOfWork.Repository<Booking>().Update(booking);
            var result = await unitOfWork.CompleteAsync();
            if (!result)
                return APIResponse<string>.FailureResponse(500, null, "Failed to update booking. Please try again later.");

            return APIResponse<string>.SuccessResponse(200, null, "Booking updated successfully.");
        }
    }
}
