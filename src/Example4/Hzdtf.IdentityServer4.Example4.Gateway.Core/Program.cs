using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hzdtf.IdentityServer4.Example4.Gateway.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureAppConfiguration(config =>
                    {
                        //config.AddJsonFile("Config/ocelot.json", false, reloadOnChange: true);
                        config.AddJsonFile("Config/ocelot_dynamic_route.json", false, reloadOnChange: true);
                    });
                });
    }
}
