using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz.Impl;
using Quartz;
using QuotesExchangeApp.Data;
using System;
using Quartz.Spi;
using QuotesExchangeApp.Jobs;
using QuotesExchangeApp.Quartz;
using QuotesExchangeApp.Controllers;

namespace QuotesExchangeApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizePage("/Properties", "RequireAdministratorRole");
                options.Conventions.AuthorizePage("/Quote");
                options.Conventions.AuthorizePage("/Chart");
                options.Conventions.AuthorizePage("/Company");
                options.Conventions.AuthorizePage("/Index");

            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("admin"));
            });
            services.AddDirectoryBrowser();
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
            services.AddMvc();
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapHub<ChartHub>("/signalr");
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
