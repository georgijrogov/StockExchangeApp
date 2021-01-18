using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuotesExchangeApp.Data;
using QuotesExchangeApp.Models;

namespace QuotesExchangeApp.Pages
{
    public class CompanyModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public List<Company> Companies { get; set; }
        //public List<PropertiesObject> PropertiesObjects { get; set; }
        public CompanyModel(ApplicationDbContext db)
        {
            _context = db;
        }
        public void OnGet()
        {
            Companies = _context.Companies.AsNoTracking().ToList();
            //PropertiesObjects = _context.PropertiesObjects.AsNoTracking().ToList();
            //int prop = 0;
            //var res = (from propertieobj in _context.PropertiesObjects.Skip(Math.Max(0, PropertiesObjects.Count() - 1))
            //           select new
            //           {
            //               PropertyID = propertieobj.Id,
            //               PropertyMinutes = propertieobj.Minutes
            //           }).ToList();
            //foreach (var r in res)
            //    prop = r.PropertyMinutes;
            //int test = prop;
            //DBUpdaterScheduler.Start();
        }
    }
}
