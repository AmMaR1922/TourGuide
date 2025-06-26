using ApplicationLayer.Contracts.Specifications;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace TourGuide.Services.CategoryService.Specification
{
    public class GetCategorySpecificationById :BaseSpecification<Category>
    {
        public GetCategorySpecificationById(int Id)
        {
            Criteria = c => c.Id == Id && !c.IsDeleted;

            AddInclude(c => c.Include(c => c.Trips));
        }
    }
}
