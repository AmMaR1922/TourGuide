using ApplicationLayer.DTOs.ApplicationUser;
using ApplicationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Contracts.Auth
{
    public interface IAuthServices
    {
        Task<APIResponse<ApplicationUserResponseDTO>> Register(ApplicationUserRegisterDTO appuser);
        Task<APIResponse<ApplicationUserResponseDTO>> Login(ApplicationUserLoginDTO appuser);

        Task<APIResponse<ApplicationUserResponseDTO>> GetNewToken(string RefreshToken);
        Task<APIResponse<string>> RevokeRefreshToken(string Token);
        
    }
}
