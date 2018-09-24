using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer
{
    public class DbInitializer
    {
        public static async Task InitializeAsyc(AppIdentityDbContext context,IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<AppIdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<AppIdentityUser>>();
            var Context = serviceProvider.GetRequiredService<AppIdentityDbContext>();

            if (!Context.Photos.Any())
            {
                Photo defProfPic = new Photo()
                {
                    Url = "http://24eastmain.com/wp-content/uploads/2017/08/test.jpg",
                    Description = "Default profile picture",
                    DateAdded = DateTime.Now,
                    IsMain = true
                };

                await Context.Photos.AddAsync(defProfPic);
                Context.SaveChanges();
            }

            if (!Context.Genders.Any())
            {
                Gender g1 = new Gender()
                {
                    Id = 1,
                    Name = "Kişi"
                };
                Gender g2 = new Gender()
                {
                    Id = 2,
                    Name = "Qadın"
                };
                await Context.Genders.AddAsync(g1);
                await Context.Genders.AddAsync(g2);
            }
            
            if (!Context.Roles.Any())
            {
                AppIdentityRole role = new AppIdentityRole()
                {
                    Name = "Admin"
                };
                await RoleManager.CreateAsync(role);
            }

            if (!Context.Users.Any())
            {
                AppIdentityUser admin = new AppIdentityUser()
                {
                    Email = "emilea@code.edu.az",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "emilea@code.edu.az",
                    GenderId = 1,
                    PhotoId = 1
                };
                await UserManager.CreateAsync(admin, "Password@123");
                await UserManager.AddToRoleAsync(admin, "Admin");
            }

        }
    }
}
