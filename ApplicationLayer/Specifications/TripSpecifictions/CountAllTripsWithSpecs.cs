using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Specifications.TripSpecifictions
{
    public class CountAllTripsWithSpecs : BaseSpecification<Trip>
    {
        public CountAllTripsWithSpecs(TripSpecParams Params)
        {
            Criteria = trip =>
                (Params.CategoryId == null || trip.CategoryId == Params.CategoryId) &&
                (Params.LanguageId == null || trip.TripLanguages.Any(tl => tl.LanguageId == Params.LanguageId)) &&
                (Params.IsBestSeller == null || trip.IsBestSeller == Params.IsBestSeller) &&
                (Params.IsTopRated == null || trip.TripReviews.Average(r => r.Rating) >= 4);
        }
        
    }
}
