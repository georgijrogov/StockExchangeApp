using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using QuotesExchangeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace QuotesExchangeApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                ScheduleJobs(services);
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        private static void ScheduleJobs(IServiceProvider services)
        {
            var scheduler = services.GetService<IScheduler>();
            var grabDelay = TimeSpan.FromMinutes(5);
            QuartzServicesUtilities.StartJob<FinnhubGrabberJob>(scheduler, grabDelay);
            QuartzServicesUtilities.StartJob<MoexGrabberJob>(scheduler, grabDelay);
        }
    }
}
