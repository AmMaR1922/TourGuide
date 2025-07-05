using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Specifications.ActivitySpecifications
{
    public class GetAllActivitiesSpecifications : BaseSpecification<Activity>
    {
        public GetAllActivitiesSpecifications(SpecParams Params)
        {
            Criteria = activity => (string.IsNullOrEmpty(Params.Search) || EF.Functions.Like(activity.Name, $"%{Params.Search}%"));
        }
    }
}
