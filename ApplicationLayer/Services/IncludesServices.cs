using ApplicationLayer.Contracts.Services;
using ApplicationLayer.Contracts.UnitToWork;
using ApplicationLayer.DTOs.Activity;
using ApplicationLayer.DTOs.Includes;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using ApplicationLayer.Specifications.IncludeSpecification;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services
{
    public class IncludesServices : IIncludesServices
    {
        private readonly IUnitOfWork unitOfWork;

        public IncludesServices(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<APIResponse<Pagination<IncludesDTOResponse>>> GetAll(SpecParams Spec)
        {
            var Specsdata = new IncludeSpecificationGetAll(Spec);
            var SpecCount = new IncludeSpecificationGetAllCount(Spec);

            var data = await unitOfWork.Repository<Includes>().GetAllWithSpecification(Specsdata)
            .Select(a => new IncludesDTOResponse()
            {
                Id = a.Id,
                Name = a.Name,
            })
            .ToListAsync();

            var count = await unitOfWork.Repository<Includes>().GetCountWithSpecs(SpecCount);

           

            return APIResponse<Pagination<IncludesDTOResponse>>.SuccessResponse(200,new Pagination<IncludesDTOResponse>(Spec.PageNumber,Spec.PageSize,count,data) , "Includes Retrieved Successfully.");
        }

        public async Task<APIResponse<IncludesDTOResponse>> GetById(int Id)
        {
            var include = await unitOfWork.Repository<Includes>().GetByIdAsync(Id);

            if (include == null)
                return APIResponse<IncludesDTOResponse>.FailureResponse(404, null, "Include not found.");

            return APIResponse<IncludesDTOResponse>.SuccessResponse(200, new IncludesDTOResponse
            {
                Id = include.Id,
                Name = include.Name,
            }, "Include Retrieved Successfully.");
        }
        
        public async Task<APIResponse<string>> Add(IncludesDTORequest IncludeDto)
        {
            var IncludeExists = unitOfWork.Repository<Includes>().GetAll().Any(a => a.Name == IncludeDto.Name);

            if (IncludeExists)
                return APIResponse<string>.FailureResponse(400, null, "Include already exists with this name.");

            var Include = new Includes() { Name = IncludeDto.Name };
            await unitOfWork.Repository<Includes>().AddAsync(Include);
            var result = await unitOfWork.CompleteAsync();

            if (!result)
                return APIResponse<string>.FailureResponse(500, null, "Failed to add Include.");
            return APIResponse<string>.SuccessResponse(200, null, "Include added successfully.");
        }

        public async Task<APIResponse<string>> Delete(int Id)
        {
            var include = await unitOfWork.Repository<Includes>().GetByIdAsync(Id);

            if (include == null)
                return APIResponse<string>.FailureResponse(404, null, "Include not found.");

            unitOfWork.Repository<Includes>().Delete(include);
            var result = await unitOfWork.CompleteAsync();

            if (!result)
                return APIResponse<string>.FailureResponse(500, null, "Failed to delete include.");

            return APIResponse<string>.SuccessResponse(200, null, "Include deleted successfully.");
        }

        public async Task<APIResponse<string>> Update(int Id, IncludesDTORequest ActivityDto)
        {
            var Include = await unitOfWork.Repository<Includes>().GetByIdAsync(Id);

            if (Include == null)
                return APIResponse<string>.FailureResponse(404, null, "Include not found.");

            Include.Name = ActivityDto.Name;
            var result = await unitOfWork.CompleteAsync();

            if (!result)
                return APIResponse<string>.FailureResponse(500, null, "Failed to update include.");

            return APIResponse<string>.SuccessResponse(200, null, "Include updated successfully.");
        }
    }
}
