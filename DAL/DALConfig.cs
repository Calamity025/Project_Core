using System.Runtime.CompilerServices;
using Autofac;
using DAL.Interfaces;
using DAL.Repositories;
using Entities;
using Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{
    public class DALConfig : Module
    {
        public static void ConfigureServices(IServiceCollection services, string connectionName)
        {
            services.AddEntityFrameworkSqlServer().AddDbContext<AuctionContext>(options =>
                options.UseSqlServer(connectionName));
            services.AddScoped<IDbContext, AuctionContext>();
            services.AddScoped<ISlotRepository, SlotRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IUserInfoRepository, UserInfoRepository>();
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AuctionContext>();
        }
    }
}
