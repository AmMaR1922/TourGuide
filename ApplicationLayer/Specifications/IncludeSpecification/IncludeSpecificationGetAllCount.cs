using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Specifications.IncludeSpecification
{
    public class IncludeSpecificationGetAllCount:BaseSpecification<Includes>
    {
        public IncludeSpecificationGetAllCount(SpecParams spec) {

            Criteria = i => (string.IsNullOrWhiteSpace(spec.Search) || EF.Functions.Like(i.Name, $"%{spec.Search}%"));

        }
    }
}
