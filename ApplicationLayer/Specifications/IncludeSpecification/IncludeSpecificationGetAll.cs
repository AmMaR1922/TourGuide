using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using ApplicationLayer.Specifications.BookingsSpecifications;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Specifications.IncludeSpecification
{
    public class IncludeSpecificationGetAll:BaseSpecification<Includes>
    {
        public IncludeSpecificationGetAll(SpecParams spec)
        {
            Criteria = i => (string.IsNullOrWhiteSpace(spec.Search) || EF.Functions.Like(i.Name, $"%{spec.Search}%"));

            IsPaginated = true;
            ApplyPagination(spec.PageSize * (spec.PageNumber - 1), spec.PageSize);
                
        }

    }
}
