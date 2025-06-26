using ApplicationLayer.Contracts.Specifications;
using ApplicationLayer.Models.SpecificationParameters;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace TourGuide.Services.TripService.Specification
{
    public class GetAllTripSpecification : BaseSpecification<Trip>
    {
        public GetAllTripSpecification(TripSpecParams Params)
        {
            Criteria = C =>
            (
                (string.IsNullOrEmpty(Params.Search) || C.Name.ToLower().Contains(Params.Search.ToLower()))
            );

            if (Params.sort == "asec")
                OrderByAsec = C => C.CreatedAt;
            else
                OrderByDesc = C => C.CreatedAt;

            IsPaginated = true;

            if (IsPaginated)
            {
                ApplyPagination((Params.PageNumber - 1) * Params.PageSize, Params.PageSize);
            }

            Includes.Add(C => C.Include(C => C.Category));
        }
    }
}
