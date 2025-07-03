using ApplicationLayer.Contracts.Services;
using ApplicationLayer.Contracts.UnitToWork;
using ApplicationLayer.DTOs.Activity;
using ApplicationLayer.DTOs.CategoryDto;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services
{
    public class ActivityServices : IActivityServices
    {
        private readonly IUnitOfWork unitOfWork;

        public ActivityServices(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<APIResponse<List<ActivityDTOResponse>>> GetAll()
        {
            var Activities = await unitOfWork.Repository<Activity>().GetAll()
                .Select(a => new ActivityDTOResponse()
                {
                    Id = a.Id,
                    Name = a.Name,
                })
                .ToListAsync();

            return APIResponse<List<ActivityDTOResponse>>.SuccessResponse(200, Activities, "Activities Retrieved Successfully.");
        }

        public async Task<APIResponse<ActivityDTOResponse>> GetById(int Id)
        {
            var activity = await unitOfWork.Repository<Activity>().GetByIdAsync(Id);

            if (activity == null)
                return APIResponse<ActivityDTOResponse>.FailureResponse(404, null, "Activity not found.");

            return APIResponse<ActivityDTOResponse>.SuccessResponse(200, new ActivityDTOResponse
            {
                Id = activity.Id,
                Name = activity.Name,
            }, "Activity Retrieved Successfully.");
        }
        
        public async Task<APIResponse<string>> Add(ActivityDTORequest ActivityDto)
        {
            var activityExists = unitOfWork.Repository<Activity>().GetAll().Any(a => a.Name == ActivityDto.Name);

            if(activityExists)
                return APIResponse<string>.FailureResponse(400, null, "Activity already exists with this name.");

            var activity = new Activity() { Name = ActivityDto.Name };
            await unitOfWork.Repository<Activity>().AddAsync(activity);
            var result = await unitOfWork.CompleteAsync();

            if (!result)
                return APIResponse<string>.FailureResponse(500, null, "Failed to add activity.");
            return APIResponse<string>.SuccessResponse(200, null, "Activity added successfully.");
        }

        public async Task<APIResponse<string>> Delete(int Id)
        {
            var activity = await unitOfWork.Repository<Activity>().GetByIdAsync(Id);

            if (activity == null)
                return APIResponse<string>.FailureResponse(404, null, "Activity not found.");

            unitOfWork.Repository<Activity>().Delete(activity);
            var result = await unitOfWork.CompleteAsync();

            if (!result)
                return APIResponse<string>.FailureResponse(500, null, "Failed to delete activity.");

            return APIResponse<string>.SuccessResponse(200, null, "Activity deleted successfully.");
        }

        public async Task<APIResponse<string>> Update(int Id, ActivityDTORequest ActivityDto)
        {
            var activity = await unitOfWork.Repository<Activity>().GetByIdAsync(Id);

            if (activity == null)
                return APIResponse<string>.FailureResponse(404, null, "Activity not found.");

            activity.Name = ActivityDto.Name;
            var result = await unitOfWork.CompleteAsync();

            if (!result)
                return APIResponse<string>.FailureResponse(500, null, "Failed to update activity.");

            return APIResponse<string>.SuccessResponse(200, null, "Activity updated successfully.");
        }
    }
}
