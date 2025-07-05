using ApplicationLayer.DTOs.Activity;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Contracts.Services
{
    public interface IActivityServices
    {
        Task<APIResponse<List<ActivityDTOResponse>>> GetAll(SpecParams Params);
        Task<APIResponse<ActivityDTOResponse>> GetById(int Id);
        Task<APIResponse<string>> Add(ActivityDTORequest ActivityDto);
        Task<APIResponse<string>> Delete(int Id);
        Task<APIResponse<string>> Update(int Id, ActivityDTORequest ActivityDto);
    }
}
