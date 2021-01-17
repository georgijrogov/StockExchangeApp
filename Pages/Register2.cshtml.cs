using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuotesExchangeApp.Data;

namespace QuotesExchangeApp.Pages
{
    public class Register2Model : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

                //Pass the userId of the user who you want to assign role to
        //await createRoles("2a5dede2-cb4c-4da3-b1d2-d8aa52028528","Admin");

        public async Task createRoles(string userId, string roleName)
        {
            bool x = await _roleManager.RoleExistsAsync(roleName);
            if (!x)//Create new role if not existing
            {
                var role = new IdentityRole();
                role.Name = roleName;
                await _roleManager.CreateAsync(role);
            }

            //Get all roles of the user by userId
            var allRoles = (from userRole in _context.UserRoles.Where(ur => ur.UserId == userId).ToList()
                            join r in _context.Roles
                            on userRole.RoleId equals r.Id
                            select r.Name).ToList();

            //assign role only if he does not have this roleName before            
            if (!allRoles.Contains(roleName))
            {
                var user = await _userManager.FindByIdAsync(userId);
                var result1 = await _userManager.AddToRoleAsync(user, roleName);
                //For removing a role, use await _userManager.RemoveFromRoleAsync(user, "Admin");
            }
        }
        public void OnGet()
        {
            createRoles("e247e2cd-267a-4c31-9669-2146eea9b5be", "Admin");
        }
    }
}
