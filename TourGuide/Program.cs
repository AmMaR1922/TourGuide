using InfrastructureLayer.Data.Context;
using InfrastructureLayer.Data.Seeding;
using Microsoft.EntityFrameworkCore;
using TourGuide.Extentions;

var builder = WebApplication.CreateBuilder(args);

ApplicationServicesExtentions.AddApplicationServices(builder.Services, builder.Configuration);
IdentityServicesExtention.AddIdentityServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var service=app.Services.CreateScope())
{
   await IdentitySeeding.IdentitySeedingOperation(service.ServiceProvider);
}

app.UseStaticFiles();
app.UseHttpsRedirection();
    
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
