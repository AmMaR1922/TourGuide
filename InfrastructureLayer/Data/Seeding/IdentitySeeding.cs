using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InfrastructureLayer.Data.Seeding
{
    public static class IdentitySeeding
    {
        public static async Task IdentitySeedingOperation(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            if(! await roleManager.Roles.AnyAsync())
            {
                string[] Roles = { "Admin", "NormalUser" };
                foreach(var role in Roles)
                {
                     if(!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole<int>(role));
                    }
                }

            }

            if(!await userManager.Users.AnyAsync())
            {
                ApplicationUser Admin = new ApplicationUser()
                {
                    Email = "victornisem01@gmail",
                    EmailConfirmed = true,
                    UserName ="Vico",
                    PhoneNumber = "01282825288"
                    
                };

                ApplicationUser NormalUser = new ApplicationUser()
                {
                    Email = "code.builders2@gmail.com",
                    EmailConfirmed = true,
                    UserName = "CodeBuilders",
                    PhoneNumber = "01282825288"

                };

                var result1 = await userManager.CreateAsync(Admin,"1234*VvV");
                var result2 = await userManager.CreateAsync(NormalUser, "1234*VvV");

                if(result1.Succeeded && result2.Succeeded)
                {
                    await userManager.AddToRoleAsync(Admin, "Admin");
                    await userManager.AddToRoleAsync(NormalUser, "NormalUser");
                }

                else
                {
                    foreach(var errors in result2.Errors)
                    {
                        Console.WriteLine($"{errors.Description}");
                    }
                    foreach (var errors in result1.Errors)
                    {
                        Console.WriteLine($"{errors.Description}");
                    }
                }




            }

        }
    }
}
