using ApplicationLayer.DTOs.CategoryDto;
using ApplicationLayer.DTOs.Trip;
using ApplicationLayer.DTOs.TripDtos;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Contracts.Services
{
    public interface ITripServices
    {
        Task<APIResponse<Pagination<TripDTOResponse>>> GetAll(TripSpecParams Params, bool isAdmin);
        Task<APIResponse<TripToBeReturnedDTO>> GetById(int Id);
        Task<APIResponse<string>> Add(TripToBeAddedDTO TripDto);
        Task<APIResponse<string>> Delete(int Id);
        Task<APIResponse<string>> Update(int Id, TripToBeUpdatedDTO TripDto);
        Task<APIResponse<string>> AddImagesToTrip(int tripId, List<IFormFile> files);
        Task<APIResponse<string>> DeleteImagesFromTrip(int tripId, List<int> imageIds);
    }
}
