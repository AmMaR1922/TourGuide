
using ApplicationLayer.Models;
using DomainLayer.Entities;
using InfrastructureLayer.Data.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TourGuide.Extentions
{
    public static class IdentityServicesExtention
    {
        public static IServiceCollection AddIdentityServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole<int>>(options => {

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

                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
               
                #region Tokens
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
                options.Tokens.ChangeEmailTokenProvider = TokenOptions.DefaultProvider; 
                #endregion

                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<TourGuideDbContext>()
                .AddDefaultTokenProviders();




            services.AddAuthentication(options =>
            {

                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;

            })
               
            .AddGoogle(o => {
                var ClientId = configuration["Google:ClientId"];

                if (ClientId is null)
                {
                    throw new ArgumentNullException(nameof(ClientId));
                }

                var ClientSecret = configuration["Google:ClientSecret"];
               
                if (ClientSecret is null)
                {
                    throw new ArgumentNullException(nameof(ClientSecret));
                }

                o.ClientId = ClientId;
                o.ClientSecret = ClientSecret;
                o.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;








            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = true;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
                    ClockSkew = TimeSpan.Zero


                };

            });

               

            services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromMinutes(10));


            services.Configure<JWT>(configuration.GetSection("JWT"));
                

            return services;
        }
    }
}
