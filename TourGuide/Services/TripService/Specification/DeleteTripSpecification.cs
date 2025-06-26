using ApplicationLayer.Contracts.Specifications;
using DomainLayer.Entities;

namespace TourGuide.Services.TripService.Specification
{
    public class DeleteTripSpecification : BaseSpecification<Trip>
    {
        public DeleteTripSpecification(int Id)
        {
            Criteria = c => c.Id == Id;


        }
    }
}
