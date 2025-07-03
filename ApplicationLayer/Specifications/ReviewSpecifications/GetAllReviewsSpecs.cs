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
    public class GetAllReviewsSpecs : BaseSpecification<TripReviews>
    {
        public GetAllReviewsSpecs(ReviewsSpecParams Params)
        {
            Criteria = x => x.TripId == Params.TripId;

            if (Params.Sort != null && Params.Sort.Any())
            {
                foreach (var sort in Params.Sort)
                {
                    if(sort == "rating:desc")
                    {
                        AddOrderBy(x => x.Rating, true);
                    }
                    else if (sort == "rating:asc")
                    {
                        AddOrderBy(x => x.Rating);
                    }
                    else if (sort == "comment:desc")
                    {
                        AddOrderBy(x => x.Comment.Count(), true);
                    }
                    else if (sort == "comment:asc")
                    {
                        AddOrderBy(x => x.Comment.Count());
                    }
                    else if (sort == "date:desc")
                    {
                        AddOrderBy(x => x.CreatedAt, true);
                    }
                    else if (sort == "date:asc")
                    {
                        AddOrderBy(x => x.CreatedAt);
                    }
                }
            }
            else
            {
                AddOrderBy(x => x.Rating, true);
            }

            IsPaginated = true;
            ApplyPagination((Params.PageNumber - 1) * Params.PageSize, Params.PageSize);
        }
    }
}
