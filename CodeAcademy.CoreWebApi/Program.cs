using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using CodeAcademy.CoreWebApi.DataAccessLayer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using Microsoft.AspNetCore.Identity;
using Serilog.Sinks.MSSqlServer;
using System.Data;
using System.Collections.ObjectModel;
using Serilog;

namespace CodeAcademy.CoreWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            using (var scope = host.Services.CreateScope())
            {
                var service = scope.ServiceProvider;
                var context = service.GetRequiredService<AppIdentityDbContext>();
                DbInitializer.InitializeAsyc(context, service).Wait();
            }
            host.Run();
        }
        public static IWebHost BuildWebHost(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
           .UseStartup<Startup>()
            .Build();
    }
}
