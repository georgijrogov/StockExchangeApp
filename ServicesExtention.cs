using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using QuotesExchangeApp.Jobs;
using QuotesExchangeApp.Quartz;

namespace QuotesExchangeApp
{
    public static class ServicesExtention
    {
        public static void ConfigureQuartz(this IServiceCollection services)
        {
            services.AddSingleton<IJobFactory, JobFactory>();

            services.Add(new ServiceDescriptor(typeof(FinnhubGrabberJob), typeof(FinnhubGrabberJob), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(MoexGrabberJob), typeof(MoexGrabberJob), ServiceLifetime.Transient));

            services.AddSingleton(provider =>
            {
                var schedulerFactory = new StdSchedulerFactory();
                var scheduler = schedulerFactory.GetScheduler().Result;
                scheduler.JobFactory = provider.GetService<IJobFactory>();

                scheduler.Start();

                return scheduler;
            });

            services.AddTransient<ISchedulerFactory, StdSchedulerFactory>();
        }
    }
}
