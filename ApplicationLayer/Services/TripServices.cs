using ApplicationLayer.Contracts.Services;
using ApplicationLayer.Contracts.UnitToWork;
using ApplicationLayer.DTOs.Trip;
using ApplicationLayer.DTOs.TripDtos;
using ApplicationLayer.Helper;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using ApplicationLayer.Specifications.TripSpecifictions;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services
{
    public class TripServices : ITripServices
    {
        private readonly IUnitOfWork unitOfWork;

        public TripServices(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<Pagination<TripDTOResponse>>> GetAll(TripSpecParams Params)
        {
            var Specs = new GetAllTripsWithSpecs(Params);
            var Trips = await unitOfWork.Repository<Trip>().GetAllWithSpecification(Specs)
                .Select(trip => new TripDTOResponse
                {
                    Id = trip.Id,
                    Name = trip.Name,
                    Duration = trip.Duration,
                    Price = trip.Price,
                    IsAvailable = trip.IsAvailable, // ternary operator based on user Role
                    DateTime = trip.DateTime,
                    Category = trip.Category.Name,
                    Rating = trip.Rating,
                    IsBestSeller = trip.IsBestSeller,
                    Reviews = trip.TripReviews.Count(),
                    MainImageURL = URLResolver.BuildFileUrl(trip.TripImages.Where(i => i.IsMainImage).Select(img => img.ImageURL).FirstOrDefault())
                })
                .ToListAsync();

            var CountSpecs = new CountAllTripsWithSpecs(Params);
            var Count = await unitOfWork.Repository<Trip>().GetCountWithSpecs(CountSpecs);

            var Pagination = new Pagination<TripDTOResponse>(Params.PageNumber, Params.PageSize, Count, Trips);

            return APIResponse<Pagination<TripDTOResponse>>.SuccessResponse(200, Pagination, "Trips retrieved successfully.");
        }

        public Task<APIResponse<TripToBeReturnedDTO>> GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<APIResponse<string>> Add(TripToBeAddedDTO TripDto)
        {
            throw new NotImplementedException();
        }

        public Task<APIResponse<string>> Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<APIResponse<string>> Update(int Id, TripToBeUpdatedDTO TripDto)
        {
            throw new NotImplementedException();
        }
    }
}
