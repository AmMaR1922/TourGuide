using ApplicationLayer.DTOs.Wishlist;
using ApplicationLayer.Models;
using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Contracts.Services
{
    public interface IWishlistServices
    {
        Task<APIResponse<List<WishlistDTOResponse>>> GetAll(ApplicationUser user);
        Task<APIResponse<string>> Add(ApplicationUser user, int TripId);
        Task<APIResponse<string>> Delete(ApplicationUser user, int TripId);


    }
}
