using System;
using System.Threading.Tasks;
using BLL.Services;
using DAL;
using DAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public class BLLConfig
    {
        public static void ConfigureServices(IServiceCollection services, string connectionName)
        {
            services.AddScoped<IDataUnitOfWork, DataUnitOfWork>();
            services.AddScoped<IIdentityUnitOfWork, IdentityUnitOfWork>();
            DALConfig.ConfigureServices(services, connectionName);
        }

        public static async Task InitializeIdentityInitializer(IServiceProvider services) =>
            await IdentityInitializerService.InitializeAsync(services);
    }
}
