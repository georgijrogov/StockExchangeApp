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
        public string Message { get; set; }
        private readonly ApplicationContext _context;
        public ChartModel(ApplicationContext db)
        {
            _context = db;
        }
        public string Json { get; set; }
        public string CompanyName { get; set; }
        public void OnGet()
        {
            Message = "Apple";
            TakeQuotes(1);
        }
        public void OnPost(string comp)
        {
            Message = comp;
            if (comp == "Apple")
                TakeQuotes(1);
            if (comp == "Tesla")
                TakeQuotes(2);
            if (comp == "AMD")
                TakeQuotes(3);
            if (comp == "Intel")
                TakeQuotes(4);
            if (comp == "Amazon")
                TakeQuotes(5);
            if (comp == "Microsoft")
                TakeQuotes(6);
        }
        public void TakeQuotes(int c)
        {
            var res = (from quote in _context.Quotes
                       where quote.Id_Company == c
                       select new
                       {
                           QuotePrice = quote.Price,
                           QuoteDate = quote.Date
                       }).ToList();
            Json = JsonConvert.SerializeObject(res);
        }
    }
}
