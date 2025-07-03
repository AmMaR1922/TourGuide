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
    public class GetAllTripsWithSpecs : BaseSpecification<Trip>
    {
        public GetAllTripsWithSpecs(TripSpecParams Params)
        {
            Criteria = trip => 
                (Params.CategoryId == null || trip.CategoryId == Params.CategoryId) &&
                (Params.LanguageId == null || trip.TripLanguages.Any(tl => tl.LanguageId == Params.LanguageId)) &&
                (Params.IsBestSeller == null || trip.IsBestSeller == Params.IsBestSeller) &&
                (Params.IsTopRated == null || trip.TripReviews.Average(r => r.Rating) >= 4);

            int seed = DateTime.Today.GetHashCode();
            var rand = new Random(seed);

            if (Params.Sort != null && Params.Sort.Any())
            {
                foreach (var order in Params.Sort)
                {
                    switch (order.ToLower())
                        {
                        case "price:asc":
                            AddOrderBy(trip => trip.Price);
                            break;
                        case "price:desc":
                            AddOrderBy(trip => trip.Price, true);
                            break;
                        case "rating:asc":
                            AddOrderBy(trip => trip.TripReviews.Average(r => r.Rating));
                            break;
                        case "date:asc":
                            AddOrderBy(trip => trip.DateTime);
                            break;
                        case "date:desc":
                            AddOrderBy(trip => trip.DateTime, true);
                            break;
                        case "bestseller":
                            AddOrderBy(trip => trip.IsBestSeller, true);
                            break;
                        case "name:asc":
                            AddOrderBy(trip => trip.Name);
                            break;
                        case "name:desc":
                            AddOrderBy(trip => trip.Name, true);
                            break;
                        default:
                            AddOrderBy(_ => rand.Next());
                            break;
                    }
                }
            }

            IsPaginated = true;

            ApplyPagination((Params.PageNumber - 1) * Params.PageSize, Params.PageSize);
        }
    }
}
