using ApplicationLayer.DTOs.CategoryDto;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Contracts.Services
{
    public interface ICategoryServices
    {
        Task<APIResponse<Pagination<CategoryDTOResponse>>> GetAll(SpecParams Params);
        Task<APIResponse<CategoryDTOResponse>> GetById(int Id);
        Task<APIResponse<string>> Add(CategoryDTORequest CategoryDto);
        Task<APIResponse<string>> Delete(int Id);
        Task<APIResponse<string>> Update(int Id, CategoryDTORequest CategoryDto);

    }
}
