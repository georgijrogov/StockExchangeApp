using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuotesExchangeApp.Data;
using QuotesExchangeApp.Models;

namespace QuotesExchangeApp.Pages
{
    public class CompanyModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IEnumerable<Company> Companies { get; set; }

        public CompanyModel(ApplicationDbContext db)
        {
            _context = db;
        }

        public async Task OnGet()
        {
            Companies = await _context.Companies.ToListAsync();
        }
    }
}
