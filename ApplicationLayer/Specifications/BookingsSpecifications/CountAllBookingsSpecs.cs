using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Specifications.BookingsSpecifications
{
    public class CountAllBookingsSpecs : BaseSpecification<Booking>
    {
        public CountAllBookingsSpecs(BookingSpecParams Params, int userId, string userRole)
        {
            Criteria = b =>
            (b.UserId == userId || userRole == "admin") &&
            (Params.TripId == null || b.TripId == Params.TripId) &&
            (Params.UserId == null || b.UserId == Params.UserId) &&
            (Params.TripStartDate == null || b.TripDate >= Params.TripStartDate) &&
            (Params.TripEndDate == null || b.TripDate <= Params.TripEndDate) &&
            (Params.CreatedStartDate == null || b.CreatedAt >= Params.CreatedStartDate) &&
            (Params.CreatedEndDate == null || b.CreatedAt <= Params.CreatedEndDate) &&
            (b.Status == Params.BookingStatus);
        }
    }
}
