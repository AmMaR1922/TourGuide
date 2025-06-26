using ApplicationLayer.Contracts.Specifications;
using DomainLayer.Entities;

namespace TourGuide.Services.CategoryService.Specification
{
    public class DeleteCategorySpecification : BaseSpecification<Category>
    {
        public DeleteCategorySpecification(int Id)
        {
            Criteria = c => c.Id == Id;

 
        }
    }
    
}
