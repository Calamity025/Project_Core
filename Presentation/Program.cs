using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using BLL.Services;
using Entities;
using Entities.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Presentation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                await BLLModule.InitializeIdentityInitializer(scope.ServiceProvider);
            }

            host.Run();
        }

        public static IWebHost CreateWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>().Build();
    }
}
