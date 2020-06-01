using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

namespace ApiConsulGateway
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    CreateHostBuilder(args).Build().Run();
        //}

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
             .UseUrls("http://localhost:9000")
             .ConfigureAppConfiguration((hostingContext, config) =>
             {
                 config
                 .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                 .AddJsonFile("ocelot.json", reloadOnChange:true , optional:false)
                 .AddEnvironmentVariables();
             })
            .ConfigureServices(services =>
            {
                services.AddOcelot()
                .AddConsul();
                // Store the configuration in consul  
                //.AddConfigStoredInConsul();
            })
            .Configure(app =>
            {
                ////var configuration = new OcelotPipelineConfiguration
                ////{
                ////    PreErrorResponderMiddleware = async (ctx, next) =>
                ////    {
                ////        Console.WriteLine(ctx.Request.GetDisplayUrl());
                ////        await next.Invoke();
                ////    }
                ////};

                //app.UseOcelot(configuration);

                app.UseOcelot().Wait();
            });
    }
}
