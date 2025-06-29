using ApplicationLayer.Contracts.Auth;
using ApplicationLayer.DTOs.ApplicationUser;
using ApplicationLayer.Models;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TourGuide.Services.AuthServices
{
    public class AuthServices(UserManager<ApplicationUser> usermanager , RoleManager<IdentityRole<int>> rolemanager , IOptions<JWT> options ) : IAuthServices
    {
        private readonly UserManager<ApplicationUser> UserManager = usermanager;
        private readonly RoleManager<IdentityRole<int>> RoleManager = rolemanager;
        private readonly JWT jWT = options.Value;

        public async Task<APIResponse<ApplicationUserResponseDTO>> GetNewToken(string RefreshToken)
        {
            var user = await UserManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens!.Any(t => t.Token == RefreshToken));
            if(user is null)
            {
                return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400,new List<string> { "Invalid Token" }, "Login agian Please For security");
            }

            var RT = user.RefreshTokens!.SingleOrDefault(rt => rt.Token == RefreshToken);
            if(! RT!.IsActive )
            {
                return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400,new List<string> { "Invalid Token" }, "Login agian Please For security");

            }
            RT.RevokedOn = DateTime.UtcNow;

            var NewRefreshToken = GetRefreshToken();
            user.RefreshTokens!.Add(NewRefreshToken);
            var result = await UserManager.UpdateAsync(user);
            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, errors, "Error While Updating User");
            }

            

            var ApplicationUserResponse = new ApplicationUserResponseDTO()
            {
                Id = user.Id,
                AccessToken =await GetAccessToken(user),
                Email = user.Email!,
                Role = (await UserManager.GetRolesAsync(user))[0],
                UserName = user.UserName!,
                RefreshToken = NewRefreshToken.Token,
                RefreshTokenExperationDate = NewRefreshToken.ExpireOn


            };



            return APIResponse<ApplicationUserResponseDTO>.SuccessResponse(200,ApplicationUserResponse, "Access Token Refreshed Succesfully");




        }

        public async Task<APIResponse<ApplicationUserResponseDTO>> Login(ApplicationUserLoginDTO appuser)
        {
            var user = await UserManager.FindByEmailAsync(appuser.Email);
            if(user is null)
            {
                return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, new List<string> { "Email or password is Incorrect" },"Failed To Login");
            }

            if (await UserManager.IsLockedOutAsync(user))
            {
                return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, new List<string> { "account is Lock out Try agian in Few minutes" }, "Failed To Login");

            }

            var LoginResult = await UserManager.CheckPasswordAsync(user, appuser.Password);

            if(!LoginResult)
            {
                 await UserManager.AccessFailedAsync(user);
                if (await UserManager.IsLockedOutAsync(user))
                {
                    return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, new List<string> { "account is Lock out Try agian in Few minutes" }, "Failed To Login");

                }
               
                return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, new List<string> { "Email or password is Incorrect" }, "Failed To Login");

            }
            



            await UserManager.ResetAccessFailedCountAsync(user);




            var ApplicationUserResponse = new ApplicationUserResponseDTO() { 
               Id = user.Id,
               Email = user.Email!,
               Role = (await UserManager.GetRolesAsync(user))[0],
               AccessToken = await GetAccessToken(user),
               UserName = user.UserName!
            
            };

            if(user.RefreshTokens!.Any(rt=>rt.IsActive))
            {
                var RFT = user.RefreshTokens!.FirstOrDefault(rt => rt.IsActive);
                ApplicationUserResponse.RefreshTokenExperationDate = RFT!.ExpireOn;
                ApplicationUserResponse.RefreshToken = RFT.Token;
            }
            else
            {
                var RFT = GetRefreshToken();
                user.RefreshTokens!.Add(RFT);
                var result= await UserManager.UpdateAsync(user);
                if(!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, errors, "Failed To Login");
                }
                ApplicationUserResponse.RefreshTokenExperationDate = RFT!.ExpireOn;
                ApplicationUserResponse.RefreshToken = RFT.Token;

            }
            return APIResponse<ApplicationUserResponseDTO>.SuccessResponse(400, ApplicationUserResponse,"User Login Succesfully");


        }

        public async Task<APIResponse<ApplicationUserResponseDTO>> Register(ApplicationUserRegisterDTO appuser)
        {
            var UserName = appuser.Email.Substring(0, appuser.Email.IndexOf('@'));
            if (await UserManager.FindByEmailAsync(appuser.Email) is not null)
            {
                return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, new List<string> { "Email Is Duplicated Change Email Please" }, "Failed To Add User");
            }
            if (await UserManager.FindByNameAsync(UserName) is not null)
            {
                return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, new List<string> { "UserName Is Duplicated Change Email Please" }, "Failed To Add User");
            }

            var user = new ApplicationUser() { 
            Email = appuser.Email,
            PhoneNumber = appuser.PhoneNumber,
            UserName = UserName
            };

           var result= await UserManager.CreateAsync(user, appuser.Password);
            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, errors, "Failed To Add User");
            }

            if(!await RoleManager.RoleExistsAsync("NormalUser"))
            {
                return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, new List<string> {"Role not Exited"}, "Failed To Add User");

            }

            result = await UserManager.AddToRoleAsync(user, "NormalUser");
            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, errors, "Failed To Add User");

            }
          
            var RefreshToken = GetRefreshToken();
            user.RefreshTokens!.Add(RefreshToken);

           result= await UserManager.UpdateAsync(user);
            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, errors, "Failed To Add User");
            }

            var ApplicationUserResponse = new ApplicationUserResponseDTO()
            {
                Id = user.Id,
                AccessToken = await GetAccessToken(user),
                Email = user.Email,
                Role = "NormalUser",
                UserName = user.UserName,
                RefreshTokenExperationDate = RefreshToken.ExpireOn,
                RefreshToken = RefreshToken.Token


            };


            return APIResponse<ApplicationUserResponseDTO>.SuccessResponse(200, ApplicationUserResponse, "User Added Succesfully");




        }

        public async Task<APIResponse<string>>RevokeRefreshToken(string Token)
        {
            var user = await UserManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == Token));
            if(user is null)
            {
                return APIResponse<string>.FailureResponse(400, new List<string> {"User Not Valid"},"Falied To LogOut");
            }
            var RT = user.RefreshTokens.SingleOrDefault(rt => rt.Token == Token);
            if(!RT.IsActive)
            {
                return APIResponse<string>.FailureResponse(400, new List<string> { "Token Is Expired" }, "Falied To LogOut");
            }
            RT.RevokedOn = DateTime.UtcNow; 

            var result = await UserManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return APIResponse<string>.FailureResponse(400, errors, "Error While Updating User");
            }

            return APIResponse<string>.SuccessResponse(200, "User LogedOut Succesfully", "User LogedOut Succesfully");

        }


        private async Task<string> GetAccessToken(ApplicationUser User)
        {
            var AuthCliams = new List<Claim> { new Claim(ClaimTypes.Email, User.Email!) };

            var roles = await UserManager.GetRolesAsync(User);
            foreach(var role in roles)
            {
                AuthCliams.Add(new Claim(ClaimTypes.Role, role));
            }

            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWT.Key));
            var signingCred = new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256);
            var JwtSecurityToken = new JwtSecurityToken(
                issuer:jWT.Issuer,
                audience:jWT.Audience,
                signingCredentials: signingCred,
                claims:AuthCliams,
                expires:DateTime.UtcNow.AddMinutes(jWT.DurationInMinutes)
                
                );
            return new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken);
           
        }

        private RefreshToken GetRefreshToken()
        {
            var refreshToken = new Byte[32];
            using var Genrator = RandomNumberGenerator.Create();
            Genrator.GetBytes(refreshToken);
            return new RefreshToken()
            {
                CreatedOn = DateTime.UtcNow,
                ExpireOn = DateTime.UtcNow.AddDays(30),
                Token = Convert.ToBase64String(refreshToken)
            };
        }

    }
}
