using ApplicationLayer.Contracts.Auth;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace TourGuide.Services.AuthServices
{
    public class ExternalAuthService(SignInManager<ApplicationUser> signIn , UserManager<ApplicationUser> usermanager) : IExternalAuthService
    {
        private readonly SignInManager<ApplicationUser> signInManager = signIn;
        private readonly UserManager<ApplicationUser> userManager = usermanager;

        public async Task<string> HandleGoogleCallbackAsync()
        {
            var info = await signInManager.GetExternalLoginInfoAsync()
                       ?? throw new InvalidOperationException("No external login info.");

            // 1) try sign-in
            var signIn = await signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, false);

            ApplicationUser user;
            if (!signIn.Succeeded)
            {
                // 2) provision user
                var email = info.Principal.FindFirstValue(ClaimTypes.Email)
                            ?? throw new Exception("Email claim missing");
                user = new ApplicationUser { UserName = email, Email = email };
                var crt = await userManager.CreateAsync(user);
                if (!crt.Succeeded)
                    throw new Exception(string.Join("; ", crt.Errors.Select(e => e.Description)));

                var log = await userManager.AddLoginAsync(user, info);
                if (!log.Succeeded)
                    throw new Exception(string.Join("; ", log.Errors.Select(e => e.Description)));
            }
            else
            {
                user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey)
                       ?? throw new Exception("Linked user not found.");
            }

            return "Google hacked";
           
        }

    }
}
