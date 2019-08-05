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
        private readonly string _connectionName;
        public DALConfig(string connectionName)
        {
            _connectionName = connectionName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            DbContextOptionsBuilder op = new DbContextOptionsBuilder();
            op.UseSqlServer(_connectionName);
            builder.RegisterType<AuctionContext>().As<IDbContext>()
                .WithParameter(new TypedParameter(typeof(DbContextOptions), op.Options));
            builder.RegisterType<SlotRepository>().As<ISlotRepository>();
        }

        public static void ConfigureServices(IServiceCollection services, string connectionName)
        {
            services.AddEntityFrameworkSqlServer().AddDbContext<AuctionContext>(options =>
                options.UseSqlServer(connectionName));
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AuctionContext>();
        }
    }
}
