using ApplicationLayer.Contracts.Auth;
using ApplicationLayer.DTOs.ApplicationUser;
using ApplicationLayer.Models;
using DomainLayer.Entities;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PeopleService.v1;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TourGuide.Controllers.AuthController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalAuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IExternalAuthService _externalAuthService;

        public ExternalAuthController(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration
            , IExternalAuthService externalAuthService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _externalAuthService = externalAuthService;
        }

       
      
        [HttpGet("google-redirect-login")]
        public IActionResult GoogleLoginRedirect()
        {
           
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleCallback))
            };
             return Challenge(
             new AuthenticationProperties { RedirectUri = Url.Action("GoogleCallback") },
             GoogleDefaults.AuthenticationScheme);
            ;
        }

        
        [HttpGet("GoogleCallback")]
        public async Task<ActionResult<APIResponse<ApplicationUserResponseDTO>>> GoogleCallback()
        {
            
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if (!result.Succeeded) return Unauthorized();

            var response = await _externalAuthService.HandelCallBack(result);
            return response;



            
        }



        




    }
}
