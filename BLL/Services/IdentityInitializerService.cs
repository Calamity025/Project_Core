using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using Entities;
using Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services
{
    public class IdentityInitializerService
    {
        public static async Task InitializeAsync(IIdentityUnitOfWork db)
        {
            string adminEmail = "vp5gor@gmail.com";
            string password = "sudoLogin_1";
            if (await db.RoleManager.FindByNameAsync("admin") == null)
            {
                await db.RoleManager.CreateAsync(new Role("admin"));
            }
            if (await db.RoleManager.FindByNameAsync("user") == null)
            {
                await db.RoleManager.CreateAsync(new Role("user"));
            }
            if (await db.UserManager.FindByNameAsync("root") == null)
            {
                User admin = new User { UserName = "root", Email = adminEmail};
                IdentityResult result = await db.UserManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await db.UserManager.AddClaimAsync(admin, new Claim(ClaimsIdentity.DefaultNameClaimType, admin.UserName));
                    await db.UserManager.AddClaimAsync(admin, new Claim("Id", admin.Id.ToString()));
                    await db.UserManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
