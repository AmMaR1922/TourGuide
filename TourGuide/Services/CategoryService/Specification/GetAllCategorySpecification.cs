using ApplicationLayer.Contracts.Specifications;
using ApplicationLayer.Models.SpecificationParameters;
using Azure;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace TourGuide.Services.CategoryService.Specification
{
    public class GetAllCategorySpecification : BaseSpecification<Category>
    {
        public GetAllCategorySpecification(CategorySpecParams Params)
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

            Includes.Add(C => C.Include(C => C.Trips));
        }
    }
}
