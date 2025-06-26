using ApplicationLayer.DTOs.CategoryDto;
using ApplicationLayer.Models;
using ApplicationLayer.Models.SpecificationParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Contracts.Repositories.CategoryRepoository
{
   public interface ICategoryRepository
    {
      public Task<APIResponse<AddCategoryDto>> AddCategory(AddCategoryDto categoryDto);
        public Task<APIResponse<List<CategoryDtoResponse>>> GetAllCategories(CategorySpecParams Params);

        public Task<APIResponse<CategoryDtoResponse>> GetCategoryById(int Id);
        public Task<APIResponse<UpdateCategoryDto>> UpdateCategory( AddCategoryDto categoryDto, int Id);
        public Task<APIResponse<CategoryDtoResponse>> DeleteCategory(int Id);


    }
}
