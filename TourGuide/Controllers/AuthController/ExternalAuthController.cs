using ApplicationLayer.Contracts.Auth;
using ApplicationLayer.DTOs.ApplicationUser;
using DomainLayer.Entities;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TourGuide.Controllers.AuthController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalAuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public ExternalAuthController(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
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
        public async Task<IActionResult> GoogleCallback()
        {
            
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if (!result.Succeeded) return Unauthorized();

            // 2) Pull email / sub
            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var sub = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email is null || sub is null)
                return BadRequest("Missing claims.");

            // 3) Find or create the user
            var user = await _userManager.FindByEmailAsync(email)
                       ?? new ApplicationUser { Email = email, UserName = email };
            if (user.Id == 0)
                await _userManager.CreateAsync(user);

            // 4) Record the external login
            var info = new UserLoginInfo(
                GoogleDefaults.AuthenticationScheme,
                providerKey: sub,
                displayName: "Google");
            await _userManager.AddLoginAsync(user, info);

           
            return Ok(new { user.Email });
        }


      
       
    }
}
