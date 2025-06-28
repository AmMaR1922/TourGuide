using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationLayer.Contracts.Services;
using ApplicationLayer.Contracts.UnitToWork;
using ApplicationLayer.DTOs.CategoryDto;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using ApplicationLayer.Specifications;
using ApplicationLayer.Specifications.CategorySpecifications;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationLayer.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryServices(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<string>> Add(CategoryToBeAddedDTO CategoryDto)
        {
            bool catergoryExists = await unitOfWork.Repository<Category>().GetAll().AnyAsync(c => c.Name == CategoryDto.Name);

            if (catergoryExists)
            {
                return APIResponse<string>.FailureResponse(404, null, "Category already exists.");
            }
            
            var category = new Category
            {
                Name = CategoryDto.Name,
            };

            await unitOfWork.Repository<Category>().AddAsync(category);
            var result = await unitOfWork.CompleteAsync();

            if (!result)
            {
                return APIResponse<string>.FailureResponse(500, null, "Failed to add category.");
            }

            return APIResponse<string>.SuccessResponse(200, null, "Category added successfully.");
        }

        public async Task<APIResponse<string>> Delete(int Id)
        {
            var category = await unitOfWork.Repository<Category>().GetByIdAsync(Id);

            if (category == null)
            {
                return APIResponse<string>.FailureResponse(404, null, "Category not found.");
            }

            category.IsDeleted = true;
            unitOfWork.Repository<Category>().Update(category);
            var result = await unitOfWork.CompleteAsync();

            if (!result)
            {
                return APIResponse<string>.FailureResponse(500, null, "Failed to delete category.");
            }

            return APIResponse<string>.SuccessResponse(200, null, "Category deleted successfully.");
        }

        public async Task<APIResponse<Pagination<CategoryDTOResponse>>> GetAll(SpecParams Params)
        {
            var Specs = new GetAllCategoriesSpecs(Params);
            var categories = await unitOfWork.Repository<Category>().GetAllWithSpecification(Specs).ToListAsync();

            var CountSpecs = new CountAllCategoriesSpecs(Params);
            var Count = await unitOfWork.Repository<Category>().GetCountWithSpecs(CountSpecs);
                
            var categoryDtos = categories.Select(c => new CategoryDTOResponse
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            var pagination = new Pagination<CategoryDTOResponse>(Params.PageNumber, Params.PageSize, Count, categoryDtos);
            
            return APIResponse<Pagination<CategoryDTOResponse>>.SuccessResponse(200, pagination, "Categories retrieved successfully.");
        }

        public async Task<APIResponse<CategoryDTOResponse>> GetById(int Id)
        {
            var category = await unitOfWork.Repository<Category>().GetByIdAsync(Id);

            if (category == null)
            {
                return APIResponse<CategoryDTOResponse>.FailureResponse(404,null, "Category not found.");
            }

            var categoryDto = new CategoryDTOResponse
            {
                Id = category.Id,
                Name = category.Name
            };

            return APIResponse<CategoryDTOResponse>.SuccessResponse(200, categoryDto, "Category retrieved successfully.");
        }

        public async Task<APIResponse<string>> Update(int Id, CategoryToBeUpdatedDTO CategoryDto)
        {
            var category = await unitOfWork.Repository<Category>().GetByIdAsync(Id);

            if (category == null)
            {
                return APIResponse<string>.FailureResponse(404, null, "Category not found.");
            }

            bool categoryExists = await unitOfWork.Repository<Category>().GetAll()
                .AnyAsync(c => c.Name == CategoryDto.Name && c.Id != Id);

            if (categoryExists)
                {
                return APIResponse<string>.FailureResponse(500, null, "Category with this name already exists.");
            }

            category.Name = CategoryDto.Name;

            unitOfWork.Repository<Category>().Update(category);
            var result = await unitOfWork.CompleteAsync();

            if (!result)
            {
                return APIResponse<string>.FailureResponse(500, null, "Failed to update category.");
            }

            return APIResponse<string>.SuccessResponse(200, null, "Category updated successfully.");
        }
    }
}
