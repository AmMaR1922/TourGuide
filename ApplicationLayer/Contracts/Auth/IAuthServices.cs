using ApplicationLayer.DTOs.ApplicationUser;
using ApplicationLayer.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Contracts.Auth
{
    public interface IAuthServices
    {
        Task<APIResponse<string>> Register(ApplicationUserRegisterDTO appuser , HttpRequest request);
        Task<APIResponse<ApplicationUserResponseDTO>> Login(ApplicationUserLoginDTO appuser , HttpRequest request);

        Task<APIResponse<ApplicationUserResponseDTO>> GetNewToken(string RefreshToken);
        Task<APIResponse<string>> RevokeRefreshToken(string Token);
        
    }
}
