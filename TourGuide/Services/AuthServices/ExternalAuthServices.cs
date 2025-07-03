using ApplicationLayer.Contracts.Auth;
using ApplicationLayer.DTOs.ApplicationUser;
using ApplicationLayer.Models;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TourGuide.Services.AuthServices
{
    public class ExternalAuthServices(UserManager<ApplicationUser> userManager,ITokenServices token , RoleManager<IdentityRole<int>> roleManager) : IExternalAuthService
    {
        private readonly ITokenServices tokenServices = token;
        private readonly UserManager<ApplicationUser> UserManager = userManager;
        private readonly RoleManager<IdentityRole<int>> RoleManager = roleManager;


        public async Task<APIResponse<ApplicationUserResponseDTO>> HandelCallBack(AuthenticateResult result)
        {
            var Email = result.Principal?.FindFirstValue(ClaimTypes.Email);
            var Sub = result.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
           


            if (Email is null || Sub is null)
            {
                return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, new List<string> { "Error While Signin With Google" }, "Failed To Login or SignUp");
            }
            var IfLoginExited = await UserManager.FindByLoginAsync("GOOGLE", Sub);

           

            if(IfLoginExited is not null)
            {
                var ApplicationUserResponse = new ApplicationUserResponseDTO()
                {
                    Id = IfLoginExited.Id,
                    Email = IfLoginExited.Email!,
                    Role = (await UserManager.GetRolesAsync(IfLoginExited)).FirstOrDefault()??"Normal User",
                    ProfilePictureURL = IfLoginExited.ProfilePictureURL,
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
                var user = await UserManager.FindByEmailAsync(Email);

                if(user is null)
                {
                    var UrlPic = result.Principal?.FindFirstValue("urn:google:picture");


                    if (!string.IsNullOrEmpty(UrlPic))
                    {

                        UrlPic = UrlPic.Replace("=s100", "=s500");


                        UrlPic = Regex.Replace(
                            UrlPic,
                            @"=s\d+(-c)?$",
                            "=s500$1",
                            RegexOptions.IgnoreCase
                        );
                    }

                    var fullname = result.Principal?.FindFirstValue(ClaimTypes.Name);

                    var UserName = Email.Substring(0,Email.IndexOf('@'));
                    var NewUser = new ApplicationUser()
                    {
                        Email = Email,
                        UserName = UserName,
                        EmailConfirmed =true,
                        ProfilePictureURL=UrlPic,
                        FullName = fullname??UserName
                        
                        
                    };


                    var RT = tokenServices.GetRefreshToken();
                    NewUser.RefreshTokens.Add(RT);
                    var resultOfAdd =  await UserManager.CreateAsync(NewUser);
                    if (!resultOfAdd.Succeeded)
                    {
                        var error = resultOfAdd.Errors.Select(e => e.Description).ToList();
                        return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, error, "Error While Adding User");
                    }

                    if (!await RoleManager.RoleExistsAsync("NormalUser"))
                    {
                        return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, new List<string> { "Role not Exited" }, "Failed To Add User");

                    }

                    var AddRoleRes = await UserManager.AddToRoleAsync(NewUser,"NormalUser");
                    if (!AddRoleRes.Succeeded)
                    {
                        var error = AddRoleRes.Errors.Select(e => e.Description).ToList();
                        return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, error, "Error While Adding User To role");
                    }

                    var ApplicationUserResponse = new ApplicationUserResponseDTO() { 
                    
                         Id=NewUser.Id,
                         Email=NewUser.Email,
                         UserName = NewUser.UserName,
                         ProfilePictureURL=NewUser.ProfilePictureURL,
                         Role = (await UserManager.GetRolesAsync(NewUser)).FirstOrDefault()??"Role",
                         AccessToken = await tokenServices.GetAccessToken(NewUser),
                         RefreshToken = RT.Token,
                         RefreshTokenExperationDate =RT.ExpireOn

                    
                    };

                    var provider = result.Ticket?.AuthenticationScheme;
                   

                    if(provider is null)
                    {
                        return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400,null,"Error While Adding User To role");

                    }

                    var info = new UserLoginInfo(provider, Sub, provider);
                    var linkResult = await UserManager.AddLoginAsync(NewUser, info);
                    if (!linkResult.Succeeded)
                    {
                        var error = linkResult.Errors.Select(e => e.Description).ToList();
                        return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, error, "Error While Adding User To role");

                    }
                   


                    return APIResponse<ApplicationUserResponseDTO>.SuccessResponse(200, ApplicationUserResponse, "User Singup Succesfully");



                }

                else
                {
                    var ApplicationUserResponse = new ApplicationUserResponseDTO()
                    {
                        Id = user.Id,
                        AccessToken = await tokenServices.GetAccessToken(user),
                        Email = user.Email,
                        UserName = user.UserName,
                        ProfilePictureURL = user.ProfilePictureURL,
                        Role  = (await UserManager.GetRolesAsync(user)).FirstOrDefault()??"Role",
                        
                    };

                    if(user.RefreshTokens.Any(rt=>rt.IsActive))
                    {
                        var RT = user.RefreshTokens.FirstOrDefault(rt => rt.IsActive);
                        ApplicationUserResponse.RefreshToken = RT.Token;
                        ApplicationUserResponse.RefreshTokenExperationDate =RT.ExpireOn;

                    }
                    else
                    {
                        var RT = tokenServices.GetRefreshToken();
                        user.RefreshTokens.Add(RT);
                        var resultup = await UserManager.UpdateAsync(user);
                        if(!resultup.Succeeded)
                        {
                            var errors = resultup.Errors.Select(e=>e.Description).ToList();
                            return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, errors, "Error While Signin or Signup");
                        }

                        ApplicationUserResponse.RefreshToken = RT.Token;
                        ApplicationUserResponse.RefreshTokenExperationDate=RT.ExpireOn;
                      

                    }

                    var provider = result.Ticket?.AuthenticationScheme;


                    if (provider is null)
                    {
                        return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, null, "Error While Adding User To role");

                    }

                    var info = new UserLoginInfo(provider, Sub, "GOOGLE");
                    var linkResult = await UserManager.AddLoginAsync(user, info);
                    if (!linkResult.Succeeded)
                    {
                        var error = linkResult.Errors.Select(e => e.Description).ToList();
                        return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, error, "Error While Adding User To role");

                    }
                    return APIResponse<ApplicationUserResponseDTO>.SuccessResponse(200, ApplicationUserResponse, "User Singup Succesfully");



                }

               
            }




        }

        private async Task<APIResponse<ApplicationUserResponseDTO>> HandelRefreshToken(ApplicationUser User,ApplicationUserResponseDTO ApplicationUserResponse)
        {
            if (User.RefreshTokens.Any(Rt => Rt.IsActive))
            {
                var RT = User.RefreshTokens.FirstOrDefault(rt => rt.IsActive);
                ApplicationUserResponse.RefreshTokenExperationDate = RT!.ExpireOn;
                ApplicationUserResponse.RefreshToken = RT.Token;

            }
            else
            {
                var RT = tokenServices.GetRefreshToken();
                User.RefreshTokens.Add(RT);
                var res = await UserManager.UpdateAsync(User);
                if (!res.Succeeded)
                {
                    var errors = res.Errors.Select(s => s.Description).ToList();
                    return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, errors, "Failed To Login or SignUp");

                }

                ApplicationUserResponse.RefreshTokenExperationDate = RT!.ExpireOn;
                ApplicationUserResponse.RefreshToken = RT.Token;
            }
            return APIResponse<ApplicationUserResponseDTO>.SuccessResponse(200, ApplicationUserResponse, "User Login Succesfully");
        }
    }
}
