using ApplicationLayer.Contracts.Auth;
using ApplicationLayer.Contracts.Services;
using ApplicationLayer.Contracts.UnitToWork;
using ApplicationLayer.Models;
using ApplicationLayer.Services;
using InfrastructureLayer;
using InfrastructureLayer.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TourGuide.Services.AuthServices;
using TourGuide.Services.EmailServices;

namespace TourGuide.Extentions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(IServiceCollection Services, IConfiguration Configuration)
        {


            // Add services to the container.

            Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            Services.AddEndpointsApiExplorer();

            #region Swagger
            Services.AddSwaggerGen(options =>
            {
                // Add Swagger doc
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Jenny Project API",
                    Version = "v1",
                    Description = "API for Real Estate, Construction, and User Management",
                    Contact = new OpenApiContact
                    {
                        Name = "Jenny Team",
                        Email = "support@jennyproject.com"
                    }
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    //Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Description = "Enter only your JWT token (e.g., abc123). 'Bearer' prefix is added automatically."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                In = ParameterLocation.Header,
                Name = "Authorization"
            },
            Array.Empty<string>()
        }
    });

                // Group by controller
                options.TagActionsBy(api => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });
            });
            #endregion

            // Add CORS policy
            Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            Services.AddDbContext<TourGuideDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<ICategoryServices, CategoryServices>();
            Services.AddScoped<ITripServices, TripServices>();
            Services.AddScoped<IActivityServices, ActivityServices>();
            Services.AddScoped<IIncludesServices, IncludesServices>();
            Services.AddScoped<ILanguageServices, LanguageServices>();
            Services.AddScoped<IReviewsServices, ReviewsServices>();





            Services.AddScoped<IExternalAuthService, ExternalAuthServices>();
            Services.AddScoped<ITokenServices, TokenServices>();
            Services.AddScoped<IAuthServices, AuthServices>();

            Services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

            Services.AddTransient<IMailingService, MailingServices>();












            return Services;
        }
    }
}
