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
    public class CountAllCategoriesSpecs : BaseSpecification<Category>
    {
        public CountAllCategoriesSpecs(SpecParams Params)
        {
            Criteria = c => EF.Functions.Like(c.Name, $"%{Params.Search}%");
        }
    }
}
