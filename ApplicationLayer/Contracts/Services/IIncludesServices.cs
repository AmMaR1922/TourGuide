using ApplicationLayer.DTOs.Includes;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Contracts.Services
{
    public interface IIncludesServices 
    {
        Task<APIResponse<Pagination<IncludesDTOResponse>>> GetAll(SpecParams spec);
        Task<APIResponse<IncludesDTOResponse>> GetById(int Id);
        Task<APIResponse<string>> Add(IncludesDTORequest ActivityDto);
        Task<APIResponse<string>> Delete(int Id);
        Task<APIResponse<string>> Update(int Id, IncludesDTORequest ActivityDto);
    }
}
