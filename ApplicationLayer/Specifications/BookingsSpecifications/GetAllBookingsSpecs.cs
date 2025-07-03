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
                    case "tripdate:asc":
                        AddOrderBy(b => b.TripDate);
                        break;
                    case "tripdate:desc":
                        AddOrderBy(b => b.TripDate, true);
                        break;
                    case "createdat:asc":
                        AddOrderBy(b => b.CreatedAt);
                        break;
                    case "createdat:desc":
                        AddOrderBy(b => b.CreatedAt, true);
                        break;
                    case "status:asc":
                        AddOrderBy(b => b.Status);
                        break;
                    case "status:desc":
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

            IsPaginated = true;

            ApplyPagination((Params.PageNumber - 1 * Params.PageSize), Params.PageSize);
        }
    }
}
