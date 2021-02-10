using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuotesExchangeApp.Data;
using QuotesExchangeApp.Hubs;
using QuotesExchangeApp.Services.Interfaces;
using QuotesExchangeApp.Services;
using QuotesExchangeApp.Models;

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

            services.Configure<FinnhubOptions>(options => Configuration.GetSection("Finnhub").Bind(options));

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

            services.ConfigureQuartz();

            services.AddMvc();
            services.AddSignalR();
            services.AddTransient<ICompaniesService, CompaniesService>();
            services.AddTransient<IQuotesService, QuotesService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
