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
        Task<APIResponse<List<ActivityDTOResponse>>> GetAll();
        Task<APIResponse<ActivityDTOResponse>> GetById(int Id);
        Task<APIResponse<string>> Add(ActivityToBeAddedDTO ActivityDto);
        Task<APIResponse<string>> Delete(int Id);
        Task<APIResponse<string>> Update(int Id, ActivityToBeUpdatedDTO ActivityDto);
    }
}
