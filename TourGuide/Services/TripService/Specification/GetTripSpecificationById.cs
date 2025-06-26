using ApplicationLayer.Contracts.Specifications;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace TourGuide.Services.TripService.Specification
{
    public class GetTripSpecificationById : BaseSpecification<Trip>
    {
        public GetTripSpecificationById(int Id)
        {
            Criteria = c => c.Id == Id && !c.IsDeleted;

            AddInclude(c => c.Include(c => c.Category));
        }
    }
}
