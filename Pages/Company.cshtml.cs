using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuotesExchangeApp.Data;
using QuotesExchangeApp.Models;
using Quartz;
using Microsoft.AspNetCore.Identity;

namespace QuotesExchangeApp.Pages
{
    public class CompanyModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public List<Company> Companies { get; set; }
        public CompanyModel(ApplicationDbContext db)
        {
            _context = db;
        }
        public void OnGet()
        {
            Companies = _context.Companies.AsNoTracking().ToList();
        }
    }
}
