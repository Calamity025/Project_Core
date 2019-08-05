using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using Autofac;
using DAL;
using DAL.Interfaces;
using Entities;
using Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public class BLLModule : Module
    {
        private readonly string _connectionName;
        public BLLModule(string connectionName)
        {
            _connectionName = connectionName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DataUnitOfWork>().As<IDataUnitOfWork>();
            builder.RegisterType<IdentityUnitOfWork>().As<IIdentityUnitOfWork>();
            builder.RegisterModule(new DALConfig(_connectionName));
        }

        public static void ConfigureServices(IServiceCollection services, string connectionName)
        {
            DALConfig.ConfigureServices(services, connectionName);
            //services.AddDbContext<AuctionContext>(context => new AuctionContext(op.Options));
            //DbContextOptionsBuilder op = new DbContextOptionsBuilder();
            //op.UseSqlServer(connectionName);
            //services.AddScoped<IDbContext>(context => new AuctionContext(op.Options));
        }
    }
}
