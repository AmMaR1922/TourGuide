using ApplicationLayer.DTOs.Review;
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
    public interface IReviewsServices
    {
        Task<APIResponse<string>> Add(ReviewDTORequest reviewDto, ApplicationUser user);
        Task<APIResponse<Pagination<ReviewDTOResponse>>> GetAll(ReviewsSpecParams Params);


    }
}
