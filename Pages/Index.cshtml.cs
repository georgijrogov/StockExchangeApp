using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;
using QuotesExchangeApp.Data;
using QuotesExchangeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IScheduler _scheduler;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public IndexModel(RoleManager<IdentityRole> roleManager)
        {
            this._roleManager = roleManager;
        }
        //public IndexModel(UserManager<IdentityUser> userManager, IScheduler sheduler)
        //{
        //    this._userManager = userManager;
        //    this._scheduler = sheduler;
        //}

        public void OnGet()
        {
            //QuartzServicesUtilities.ChangeJobInterval<DBUpdater>(_scheduler, TimeSpan.FromMinutes(1));
            CreateRole();
            //GiveRole();
            RedirectToPage("Company");
        }
        public async Task CreateRole()
        {
            bool x = await _roleManager.RoleExistsAsync("admin");
            if (!x)
            {
                // first we create Admin role 
                var role = new IdentityRole
                {
                    Name = "admin"
                };
                await _roleManager.CreateAsync(role);
            }
        }
        public async Task GiveRole()
        {
            var user = await _userManager.FindByEmailAsync("test@gmail.com");
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, "admin");
            }
        }
    }
}
