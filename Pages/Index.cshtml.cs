using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager; 
        private readonly RoleManager<IdentityRole> _roleManager;
        //public IndexModel(RoleManager<IdentityRole> roleManager)
        //{
        //    this._roleManager = roleManager;
        //}
        //public IndexModel(UserManager<IdentityUser> userManager)
        //{
        //    this._userManager = userManager;
        //}
        public void OnGet()
        {
            //CreateRole(); //Создание роли
            //GiveRole(); //Дать роль пользователю
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
