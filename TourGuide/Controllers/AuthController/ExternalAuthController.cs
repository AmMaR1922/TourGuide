using ApplicationLayer.Contracts.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TourGuide.Controllers.AuthController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalAuthController(IExternalAuthService authService) : ControllerBase
    {
        private readonly IExternalAuthService _authService =authService;
        [HttpGet("google-signin")]
        [AllowAnonymous]
        public IActionResult GoogleSignIn()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleCallback))
            };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        // b) Handle Google’s redirect → provision/link user → return our JWT
        [HttpGet("google-callback")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleCallback()
        {
            try
            {
                var jwt = await _authService.HandleGoogleCallbackAsync();
                return Ok(new { access_token = jwt });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
