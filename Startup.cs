using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz.Impl;
using Quartz;
using QuotesExchangeApp.Data;
using QuotesExchangeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz.Spi;
using Microsoft.Extensions.FileProviders;
using System.IO;

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
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                RequestPath = "/static"
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
