using ApplicationLayer.Contracts.Repositories.CategoryRepoository;
using ApplicationLayer.Contracts.UnitToWork;
using ApplicationLayer.DTOs.CategoryDto;
using ApplicationLayer.Models;
using ApplicationLayer.Models.SpecificationParameters;
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

     

        public async Task<APIResponse<AddCategoryDto>> AddCategory(AddCategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                return APIResponse<AddCategoryDto>.FailureResponse(new List<string> { "Category data cannot be null." }, "Failed to add category.");
            }

            var categoryRepository = UnitOfWork.Repository<Category>();
 
            bool categoryExists = categoryRepository.GetAll()
                .Any(c => c.Name.ToLower() == categoryDto.Name.ToLower() && !c.IsDeleted);

            if (categoryExists)
            {
                return APIResponse<AddCategoryDto>.FailureResponse(new List<string> { "Category with this name already exists." }, "Failed to add category.");
            }

            var category = new Category
            {
                Name = categoryDto.Name
            };

            await categoryRepository.AddAsync(category);
            var saveResult = await UnitOfWork.CompleteAsync();
            if (!saveResult)
            {
                return APIResponse<AddCategoryDto>.FailureResponse(new List<string> { "Failed to save changes to the database." }, "Failed to add category.");
            }
            return APIResponse<AddCategoryDto>.SuccessResponse(categoryDto, "Category added successfully.");

        }

        public async Task<APIResponse<List<CategoryDtoResponse>>> GetAllCategories(CategorySpecParams Params)
        {
            var categoryRepository = UnitOfWork.Repository<Category>();
            var spec = new Specification.GetAllCategorySpecification(Params);
            var categories = categoryRepository.GetAllWithSpecification(spec).Where(c => !c.IsDeleted);  
            if (categories == null || !categories.Any())
            {
                return  APIResponse<List<CategoryDtoResponse>>.FailureResponse(new List<string> { "No categories found." }, "Failed to retrieve categories.");
            }
            var categoryDtos = categories.Select(c => new CategoryDtoResponse
            {
                Id = c.Id,
                Name = c.Name,
                CreatedAt = c.CreatedAt
            }).ToList();
            return APIResponse<List<CategoryDtoResponse>>.SuccessResponse(categoryDtos, "Categories retrieved successfully.");

        }

        public async Task<APIResponse<CategoryDtoResponse>> GetCategoryById(int Id)
        {
            var categoryRepository = UnitOfWork.Repository<Category>();
            var spec = new Specification.GetCategorySpecificationById(Id);
            var category = categoryRepository.GetAllWithSpecification(spec).FirstOrDefault();  
            if (category == null)
            {
                return APIResponse<CategoryDtoResponse>.FailureResponse(new List<string> { "Category not found." }, "Failed to retrieve category.");
            }
            var categoryDto = new CategoryDtoResponse
            {
                Id = category.Id,
                Name = category.Name,
                CreatedAt = category.CreatedAt
            };
            return APIResponse<CategoryDtoResponse>.SuccessResponse(categoryDto, "Category retrieved successfully.");
        }


        public async Task<APIResponse<UpdateCategoryDto>> UpdateCategory(AddCategoryDto categoryDto, int Id)
        {
            if (categoryDto == null)
            {
                return APIResponse<UpdateCategoryDto>.FailureResponse(new List<string> { "Category data cannot be null." }, "Failed to update category.");
            }
            var categoryRepository = UnitOfWork.Repository<Category>();
            var existingCategory = await categoryRepository.GetByIdAsync(Id);  
            if (existingCategory == null || existingCategory.IsDeleted)
            {
                return APIResponse<UpdateCategoryDto>.FailureResponse(new List<string> { "Category not found." }, "Failed to update category.");
            }
            existingCategory.Name = categoryDto.Name;
            categoryRepository.Update(existingCategory);  
            var saveResult = await UnitOfWork.CompleteAsync();  
            if (!saveResult)
            {
                return APIResponse<UpdateCategoryDto>.FailureResponse(new List<string> { "Failed to save changes to the database." }, "Failed to update category.");
            }

            return APIResponse<UpdateCategoryDto>.SuccessResponse(new UpdateCategoryDto { Id = existingCategory.Id, Name = existingCategory.Name }, "Category updated successfully.");
        }

        public async Task<APIResponse<CategoryDtoResponse>> DeleteCategory(int Id)
        {
            var categoryRepository = UnitOfWork.Repository<Category>();
            var existingCategory = categoryRepository.GetByIdAsync(Id).Result;
            if (existingCategory == null || existingCategory.IsDeleted)
            {
                return  APIResponse<CategoryDtoResponse>.FailureResponse(new List<string> { "Category not found." }, "Failed to delete category.");
            }
            existingCategory.IsDeleted = true;
            categoryRepository.Update(existingCategory);
            var saveResult = UnitOfWork.CompleteAsync().Result;
            if (!saveResult)
            {
                return APIResponse<CategoryDtoResponse>.FailureResponse(new List<string> { "Failed to save changes to the database." }, "Failed to delete category.");
            }
            var deletedCategoryDto = new CategoryDtoResponse
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                CreatedAt = existingCategory.CreatedAt
            };
            return APIResponse<CategoryDtoResponse>.SuccessResponse(deletedCategoryDto, "Category deleted successfully.");

        }
    }
}
 