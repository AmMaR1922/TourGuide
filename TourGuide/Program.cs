using ApplicationLayer.Helper;
using InfrastructureLayer.Data.Context;
using InfrastructureLayer.Data.Seeding;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TourGuide.Extentions;

var builder = WebApplication.CreateBuilder(args);

ApplicationServicesExtentions.AddApplicationServices(builder.Services, builder.Configuration);
IdentityServicesExtention.AddIdentityServices(builder.Services, builder.Configuration);
builder.Services.AddHttpContextAccessor();





var app = builder.Build();
app.UseCors("AllowAllOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();


    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TourGuideHurghada API v1");
        c.RoutePrefix = string.Empty;
    });
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


using (var service=app.Services.CreateScope())
{
   await IdentitySeeding.IdentitySeedingOperation(service.ServiceProvider);
    URLResolver.Init(service.ServiceProvider.GetRequiredService<IHttpContextAccessor>());

}

app.UseStaticFiles();
app.UseRouting();
app.UseHttpsRedirection();
app.UseCookiePolicy();
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
