using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Specifications.CategorySpecifications
{
    public class GetAllCategoriesSpecs : BaseSpecification<Category>
    {
        public GetAllCategoriesSpecs(SpecParams Params)
        {
            Criteria = c => EF.Functions.Like(c.Name, $"%{Params.Search}%");

            IsPaginated = true;

            ApplyPagination((Params.PageNumber - 1) * Params.PageSize, Params.PageSize);
        }
    }
}
