using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace CodeAcademy.CoreWebApi.DataAccessLayer
{
    //public class SeedDatabase
    //{
    //    public static async void Initialize(IServiceProvider serviceProvider)
    //    {
    //        var context = serviceProvider.GetRequiredService<AppIdentityDbContext>();
    //        var userManager = serviceProvider.GetRequiredService<UserManager<AppIdentityUser>>();
    //        var roleManager = serviceProvider.GetRequiredService<RoleManager<AppIdentityRole>>();
    //        context.Database.EnsureCreated();

    //        if (!context.Roles.Any())
    //        {
    //            AppIdentityRole role = new AppIdentityRole()
    //            {
    //                Name = "Admin"
    //            };
    //            await roleManager.CreateAsync(role);
    //        }
    //        if (!context.Users.Any())
    //        {
    //            AppIdentityUser user = new AppIdentityUser()
    //            {
    //                Email = "emilea@code.az",
    //                SecurityStamp = Guid.NewGuid().ToString(),
    //                UserName = "emilea"
    //            };
    //            //await userManager.CreateAsync(user, "Password@123");
    //            //await userManager.AddToRoleAsync(user, "Admin");
    //        }
    //    }
    //}
}
