using ApplicationLayer.Contracts.Repositories;
using ApplicationLayer.Contracts.Repositories.CategoryRepoository;
using ApplicationLayer.Contracts.UnitToWork;
using InfrastructureLayer;
using InfrastructureLayer.Data.Context;
using Microsoft.EntityFrameworkCore;
using TourGuide.Services.CategoryService;
using TourGuide.Services.TripService;

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
            Services.AddScoped<ICategoryRepository, CategoryService>();
            Services.AddScoped<ITripRepository, TripService>();






            return Services;
        }
    }
}
