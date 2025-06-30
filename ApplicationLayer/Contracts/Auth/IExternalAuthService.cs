using ApplicationLayer.DTOs.ApplicationUser;
using ApplicationLayer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Contracts.Auth
{
    public interface IExternalAuthService
    {
        Task<APIResponse<ApplicationUserResponseDTO>> HandelCallBack(AuthenticateResult result);
    }
}
