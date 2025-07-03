using ApplicationLayer.DTOs.Booking;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Contracts.Services
{
    public interface IBookingServices
    {
        Task<APIResponse<Pagination<BookingDTOResponse>>> GetAll(BookingSpecParams Params, ApplicationUser user);
        Task<APIResponse<BookingDTOResponse>> GetById(int Id, ApplicationUser user);
        Task<APIResponse<string>> Add(BookingToBeAddedDTO BookingDto, ApplicationUser user);
        Task<APIResponse<string>> Delete(int bookingId, ApplicationUser user);
        Task<APIResponse<string>> Update(int Id, BookingToBeUpdatedDTO BookingDto, ApplicationUser user);
    }
}
