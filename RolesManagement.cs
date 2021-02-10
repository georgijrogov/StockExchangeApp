using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace QuotesExchangeApp
{
    public class RolesManagement
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesManagement(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
        }

        public async Task OnGet()
        {
            await CreateRole(); //Создание роли
            await GiveRole(); //Дать роль пользователю
        }

        public async Task CreateRole()
        {
            bool x = await _roleManager.RoleExistsAsync("admin");
            if (!x)
            {
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
