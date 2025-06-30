using ApplicationLayer.Contracts.Auth;
using ApplicationLayer.DTOs.ApplicationUser;
using ApplicationLayer.Models;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace TourGuide.Services.AuthServices
{
    public class ExternalAuthServices(UserManager<ApplicationUser> userManager,ITokenServices token) : IExternalAuthService
    {
        private readonly ITokenServices tokenServices = token;
        private readonly UserManager<ApplicationUser> UserManager = userManager;


        public async Task<APIResponse<ApplicationUserResponseDTO>> HandelCallBack(AuthenticateResult result)
        {
            var Email = result.Principal?.FindFirstValue(ClaimTypes.Email);
            var Sub = result.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Email is null || Sub is null)
            {
                return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, new List<string> { "Error While Signin With Google" }, "Failed To Login or SignUp");
            }
            var IfLoginExited = await UserManager.FindByLoginAsync("Google", Sub);
            if(IfLoginExited is not null)
            {
                var ApplicationUserResponse = new ApplicationUserResponseDTO()
                {
                    Id = IfLoginExited.Id,
                    Email = IfLoginExited.Email!,
                    Role = (await UserManager.GetRolesAsync(IfLoginExited)).FirstOrDefault()??"Normal User",
                    AccessToken = await tokenServices.GetAccessToken(IfLoginExited),
                    UserName = IfLoginExited.UserName!

                };

                if(IfLoginExited.RefreshTokens.Any(Rt=>Rt.IsActive))
                {
                    var RT = IfLoginExited.RefreshTokens.FirstOrDefault(rt => rt.IsActive);
                    ApplicationUserResponse.RefreshTokenExperationDate = RT!.ExpireOn;
                    ApplicationUserResponse.RefreshToken = RT.Token;

                }
                else
                {
                    var RT = tokenServices.GetRefreshToken();
                    IfLoginExited.RefreshTokens.Add(RT);
                    var res = await UserManager.UpdateAsync(IfLoginExited);
                    if(!res.Succeeded)
                    {
                        var errors = res.Errors.Select(s => s.Description).ToList(); 
                        return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, errors, "Failed To Login or SignUp");

                    }

                    ApplicationUserResponse.RefreshTokenExperationDate = RT!.ExpireOn;
                    ApplicationUserResponse.RefreshToken = RT.Token;
                }
                return APIResponse<ApplicationUserResponseDTO>.SuccessResponse(200, ApplicationUserResponse, "User Login Succesfully");
            }
            else
            {
                throw new NotImplementedException();
            }




        }
    }
}
