using ApplicationLayer.Contracts.Repositories.CategoryRepoository;
using ApplicationLayer.Contracts.UnitToWork;
using ApplicationLayer.DTOs.CategoryDto;
using ApplicationLayer.Models;
using DomainLayer.Entities;

namespace TourGuide.Services.CategoryService
{
    public class CategoryService : ICategoryRepository
    {
        public IUnitOfWork UnitOfWork { get; }

        public CategoryService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        //    public async Task<APIResponse<AddCategoryDto>> AddCategory(AddCategoryDto categoryDto)
        //    {


        //    }
        //}
    }
}