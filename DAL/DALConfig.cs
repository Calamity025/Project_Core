using DAL.Interfaces;
using Entities;
using Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{
    public class DALConfig
    {
        public static void ConfigureServices(IServiceCollection services, string connectionName)
        {
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<AuctionContext>(options =>
                    options.UseSqlServer(connectionName));
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AuctionContext>()
                .AddUserManager<User>()
                .AddRoleManager<Role>();

            services.AddScoped<IDbContext, AuctionContext>();
        }
    }
}
