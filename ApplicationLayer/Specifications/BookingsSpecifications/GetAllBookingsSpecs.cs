using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using DomainLayer.Entities;
using DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Specifications.BookingsSpecifications
{
    public class GetAllBookingsSpecs : BaseSpecification<Booking>
    {
        public GetAllBookingsSpecs(BookingSpecParams Params, int userId, string userRole)
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

            if (Params.Sort != null)
            {
                switch (Params.Sort.ToLower())
                {
                    case "tripdateasec":
                        AddOrderBy(b => b.TripDate);
                        break;
                    case "tripdatedesc":
                        AddOrderBy(b => b.TripDate, true);
                        break;
                    case "createdatasec":
                        AddOrderBy(b => b.CreatedAt);
                        break;
                    case "createdatdesc":
                        AddOrderBy(b => b.CreatedAt, true);
                        break;
                    case "statusasec":
                        AddOrderBy(b => b.Status);
                        break;
                    case "statusdesc":
                        AddOrderBy(b => b.Status, true);
                        break;
                    default:
                        AddOrderBy(b => b.CreatedAt, true);
                        break;
                }
            }
            else
            {
                AddOrderBy(b => b.CreatedAt, true);
            }

            ApplyPagination((Params.PageNumber - 1 * Params.PageSize), Params.PageSize);
        }
    }
}
