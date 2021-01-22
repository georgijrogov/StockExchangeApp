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

namespace QuotesExchangeApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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
                options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Admin"));
            });
            
            services.AddSingleton<IJobFactory, JobFactory>();
            //services.AddTransient<DBUpdater>();
            services.Add(new ServiceDescriptor(typeof(DBUpdater), typeof(DBUpdater), ServiceLifetime.Transient));            
            //var container = services.BuildServiceProvider();

            //var jobFactory = new JobFactory(container);


            // Create a Quartz.NET scheduler
            //var schedulerFactory = new StdSchedulerFactory();
            //var scheduler = schedulerFactory.GetScheduler().Result;

            // Tell the scheduler to use the custom job factory
            //scheduler.JobFactory = jobFactory;
            //scheduler.JobFactory = provider.GetService<IJobFactory>();
            services.AddSingleton(provider =>
            {

                var schedulerFactory = new StdSchedulerFactory();
                var scheduler = schedulerFactory.GetScheduler().Result;
                scheduler.JobFactory = provider.GetService<IJobFactory>();

                scheduler.Start();

                return scheduler;
            });
            services.AddTransient<ISchedulerFactory, StdSchedulerFactory>();
            //DBUpdaterScheduler.Start();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

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
