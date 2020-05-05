using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Auth.Api.Infrastructure;
using Auth.Api.Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Auth.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var assemblyName = typeof(Startup).GetTypeInfo().Assembly.FullName;

            return Host.CreateDefaultBuilder(args)
                // .UseDefaultServiceProvider(options =>
                //     options.ValidateScopes = false) //If this is not here MediatR will not work
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup(assemblyName); });
        }
    }
}
