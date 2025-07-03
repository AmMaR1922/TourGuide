using ApplicationLayer.Models;
using DomainLayer.Entities;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PeopleService.v1;
using Google.Apis.Services;
using InfrastructureLayer.Data.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace TourGuide.Extentions
{
    public static class IdentityServicesExtention
    {
        public static IServiceCollection AddIdentityServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
            {

                #region Password
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                #endregion

                #region LockOut
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                #endregion

                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                #region Tokens
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
                options.Tokens.ChangeEmailTokenProvider = TokenOptions.DefaultProvider;
                #endregion

                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<TourGuideDbContext>()
                .AddDefaultTokenProviders();

            //services.ConfigureExternalCookie(options =>
            //{
            //    options.Cookie.Name = "TourGuide.External";
            //    options.Cookie.SameSite = SameSiteMode.None;
            //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            //});
            //services.ConfigureApplicationCookie(opts =>
            //{
            //    opts.LoginPath = "/api/externalauth/google-redirect-login";
            //    opts.Cookie.SameSite = SameSiteMode.None;
            //    opts.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            //    opts.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            //});




            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
               
                //options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
            {
                o.RequireHttpsMetadata = true;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                                                  Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            })
           
            .AddGoogle(GoogleDefaults.AuthenticationScheme, google =>
            {
                google.ClientId = configuration["Google:ClientId"]
                              ?? throw new ArgumentNullException(nameof(configuration));
                google.ClientSecret = configuration["Google:ClientSecret"]
                    ?? throw new ArgumentNullException(nameof(configuration));
                google.CallbackPath = "/signin-google";
                google.SaveTokens = true;
                google.UserInformationEndpoint =
            "https://www.googleapis.com/oauth2/v2/userinfo";

                
                google.CorrelationCookie.SameSite = SameSiteMode.None;
                google.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;


                google.Scope.Add("openid");
                google.Scope.Add("email");
                google.Scope.Add("profile");

              

                google.ClaimActions.Clear();              
                google.ClaimActions.MapJsonKey(
                    claimType: ClaimTypes.NameIdentifier,  
                    jsonKey: "id");
                google.ClaimActions.MapJsonKey(
                    claimType: ClaimTypes.Email,           
                    jsonKey: "email");

               
                google.ClaimActions.MapJsonKey("urn:google:picture", "picture");









                google.SignInScheme = IdentityConstants.ExternalScheme;
            });





         

            services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromMinutes(10));


            services.Configure<JWT>(configuration.GetSection("JWT"));


            return services;
        }
    }
}
