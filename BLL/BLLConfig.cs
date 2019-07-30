using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using Autofac;
using DAL;
using DAL.Interfaces;
using Entities;
using Entities.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public static class BLLModule
    {
        public static void ConfigureServices(IServiceCollection service, string connectionName)
        {
            DALConfig.ConfigureServices(service, connectionName);
        }
    }
}
