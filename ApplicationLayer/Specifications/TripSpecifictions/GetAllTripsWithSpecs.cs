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
                (Params.IsTopRated == null || trip.Rating >= 4);

            if (Params.Sort != null && Params.Sort.Any())
            {
                foreach (var order in Params.Sort)
                {
                    switch (order.ToLower())
                        {
                        case "priceasec":
                            AddOrderBy(trip => trip.Price);
                            break;
                        case "pricedesc":
                            AddOrderBy(trip => trip.Price, true);
                            break;
                        case "ratingasec":
                            AddOrderBy(trip => trip.Rating);
                            break;
                        case "datetimeasec":
                            AddOrderBy(trip => trip.DateTime);
                            break;
                        case "datetimedesc":
                            AddOrderBy(trip => trip.DateTime, true);
                            break;
                        case "bestseller":
                            AddOrderBy(trip => trip.IsBestSeller, true);
                            break;
                        case "nameasec":
                            AddOrderBy(trip => trip.Name);
                            break;
                        case "namedesc":
                            AddOrderBy(trip => trip.Name, true);
                            break;
                        default:
                            AddOrderBy(trip => Guid.NewGuid());
                            break;
                    }
                }
            }

            IsPaginated = true;

            ApplyPagination((Params.PageNumber - 1) * Params.PageSize, Params.PageSize);
        }
    }
}
