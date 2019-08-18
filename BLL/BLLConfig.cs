using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using BLL.Services;
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
        public static void ConfigureServices(IServiceCollection services, string connectionName)
        {
            services.AddScoped<IDataUnitOfWork, DataUnitOfWork>();
            services.AddScoped<IIdentityUnitOfWork, IdentityUnitOfWork>();
            DALConfig.ConfigureServices(services, connectionName);
        }

        public static async Task InitializeIdentityInitializer(IServiceProvider services)
        {
            await IdentityInitializerService.InitializeAsync(services);
        }
    }
}
