using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Specifications.ReviewSpecifications
{
    public class CountAllReviewsSpecs : BaseSpecification<TripReviews>
    {
        public CountAllReviewsSpecs(ReviewsSpecParams Params)
        {
            Criteria = r => r.TripId == Params.TripId;
        }
    }
}
