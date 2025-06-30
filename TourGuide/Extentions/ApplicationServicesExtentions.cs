using ApplicationLayer.Contracts.Auth;
using ApplicationLayer.Contracts.Services;
using ApplicationLayer.Contracts.UnitToWork;
using ApplicationLayer.Services;
using InfrastructureLayer;
using InfrastructureLayer.Data.Context;
using Microsoft.EntityFrameworkCore;
using TourGuide.Services.AuthServices;

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
            Services.AddSwaggerGen();

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
            Services.AddScoped<IExternalAuthService, ExternalAuthServices>();
            Services.AddScoped<ITokenServices, TokenServices>();




            Services.AddScoped<IAuthServices, AuthServices>();
          






            return Services;
        }
    }
}
