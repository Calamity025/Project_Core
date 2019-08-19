using System;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using Entities;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Services
{
    public class IdentityInitializerService
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var identityService = services.GetService<IIdentityService>();
            var db = services.GetService<IIdentityUnitOfWork>();
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
                var user = new IdentityCreationDTO()
                    {Email = adminEmail, Password = password, UserName = "root"};
                await identityService.Register(user);
                await identityService.AddToRole(user.UserName, "admin");
            }
        }
    }
}
