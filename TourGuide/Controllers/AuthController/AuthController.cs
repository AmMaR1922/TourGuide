using ApplicationLayer.Contracts.Auth;
using ApplicationLayer.DTOs.ApplicationUser;
using ApplicationLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations.Rules;
using System.ComponentModel;
using System.Formats.Asn1;

namespace TourGuide.Controllers.AuthController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthServices auth) : ControllerBase
    {
        private readonly IAuthServices authServices =auth;

        [HttpPost("Register")]
        public async Task<ActionResult<APIResponse<ApplicationUserResponseDTO>>> Register([FromBody]ApplicationUserRegisterDTO registerDTO)
        {
            if(!ModelState.IsValid)
            {
                var errors=ModelState.Values.SelectMany(e => e.Errors).Select(e=>e.ErrorMessage).ToList();
                return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, errors, "Failed To Add User");
            }
            var result = await authServices.Register(registerDTO);

            if(result.Succeeded && result.Data!.RefreshToken is not null)
            {
                SetRefreshTokeninCookie(result.Data.RefreshToken, result.Data.RefreshTokenExperationDate);
            }
            return result;
            
        }

        [HttpPost("Login")]
        public async Task<ActionResult<APIResponse<ApplicationUserResponseDTO>>>Login([FromBody]ApplicationUserLoginDTO userLoginDTO)
        {
            if(!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage).ToList();
                return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, errors, "Failed To Login");
            }
            var result =await authServices.Login(userLoginDTO);
            if (result.Succeeded)
            {
                SetRefreshTokeninCookie(result.Data!.RefreshToken, result.Data.RefreshTokenExperationDate);
            }
            return result;
        }

        [HttpPost("GetToken")]
        public async Task<ActionResult<APIResponse<ApplicationUserResponseDTO>>> GetNewToken()
        {
            var RefreshToken = Request.Cookies["RefreshToken"];
            if(RefreshToken is null)
            {
                return APIResponse<ApplicationUserResponseDTO>.FailureResponse(400, new List<string> { "Error While get Refresh Token" }, "Login Again For Security");
            }
            var result = await authServices.GetNewToken(RefreshToken);
            if(result.Succeeded)
            {
                SetRefreshTokeninCookie(result.Data!.RefreshToken, result.Data.RefreshTokenExperationDate);
            }
          

            return result;

        }
        [HttpPost("LogOut")]
        public async Task<ActionResult<APIResponse<string>>> LogOut([FromBody] RefreshTokenDTO ? RefreshToken)
        {
            var Token = RefreshToken?.Token ?? Request.Cookies["RefreshToken"];
            if (Token is null)
            {
                return APIResponse<string>.FailureResponse(400,new List<string> { "There is No token" }, "Failed to Logout");
            }
            var result = await authServices.RevokeRefreshToken(Token);
            if(result.Succeeded)
            {
                Response.Cookies.Delete("RefreshToken");
            }
            return result;
        }




        [ApiExplorerSettings(IgnoreApi =true)]
        private void SetRefreshTokeninCookie(string RefreshToken, DateTime Expire)
        {
            var CookieOptions = new CookieOptions()
            {
                Expires = Expire,
                Secure = true,
                HttpOnly = true

            };

            Response.Cookies.Append("RefreshToken", RefreshToken, CookieOptions);
        }
    }
}
