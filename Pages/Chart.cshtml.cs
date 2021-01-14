using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using QuotesApp.Models;

namespace QuotesApp.Pages
{
    public class ChartModel : PageModel
    {
        private readonly ApplicationContext _context;
        public ChartModel(ApplicationContext db)
        {
            _context = db;
        }
        public string Json { get; set; }
        public void OnGet()
        {
            var res = (from quote in _context.Quotes
                       where quote.Id_Company == 1
                       select new
                       {
                           QuotePrice = quote.Price,
                           QuoteDate = quote.Date
                       }).ToList();
            Json = JsonConvert.SerializeObject(res);
            int i = 1;
        }
    }
}
