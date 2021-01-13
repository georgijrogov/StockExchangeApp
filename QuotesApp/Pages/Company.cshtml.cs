using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuotesApp.Models;

namespace QuotesApp.Pages
{
    public class CompanyModel : PageModel
    {
        private readonly ApplicationContext _context;
        public List<Company> Companies { get; set; }
        public CompanyModel(ApplicationContext db)
        {
            _context = db;
        }
        public void OnGet()
        {
            Companies = _context.Companies.AsNoTracking().ToList();
        }
    }
}
