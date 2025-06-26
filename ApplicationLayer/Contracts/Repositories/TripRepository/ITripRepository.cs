using ApplicationLayer.DTOs.TripDtos;
using ApplicationLayer.Models;
using ApplicationLayer.Models.SpecificationParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Contracts.Repositories
{
    public interface ITripRepository
    {
      public  Task<APIResponse<AddTripDto>> AddTrip(AddTripDto tripDto);
        public Task<APIResponse<List<TripDtoResponse>>> GetAllTrips(TripSpecParams Params);
        public Task<APIResponse<TripDtoResponse>> GetTripById(int Id);
        public Task<APIResponse<UpdateTripDto>> UpdateTrip(UpdateTripDto tripDto, int Id);
        public Task<APIResponse<TripDtoResponse>> DeleteTrip(int Id);
        public Task<APIResponse<List<TripDtoResponse>>> GetTripsByCategoryId(int categoryId);
        public Task<APIResponse<List<TripDtoResponse>>> GetTripsByCategoryName(string categoryName);
        public Task<APIResponse<List<TripDtoResponse>>> GetTripsByRating(double rating);
    }
}
