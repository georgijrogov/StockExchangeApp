using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using QuotesExchangeApp.Data;

namespace QuotesExchangeApp.Pages
{
    public class ChartModel : PageModel
    {
        public string Message { get; set; }
        private readonly ApplicationDbContext _context;
        public ChartModel(ApplicationDbContext db)
        {
            _context = db;
        }
        public string Json { get; set; }
        public void OnGet()
        {
            Message = "Apple";
            TakeQuotes(1, 1);
        }
        public void OnPost(string comp)
        {
            SetValues(1, comp);
        }
        public void OnPostDay(string comp)
        {
            SetValues(1, comp);
        }
        public void OnPostWeek(string comp)
        {
            SetValues(7, comp);
        }
        public void OnPostMonth(string comp)
        {
            SetValues(30, comp);
        }
        public void TakeQuotes(int c, int d)
        {
            var res = (from quote in _context.Quotes
                       where quote.Id_Company == c && quote.Date > DateTime.Now.AddDays(-d)
                       orderby quote.Date
                       select new
                       {
                           QuotePrice = quote.Price,
                           QuoteDate = quote.Date
                       }).ToList();
            Json = JsonConvert.SerializeObject(res);
        }
        public void SetValues(int days, string message)
        {
            Message = message;
            if (message == "Apple")
                TakeQuotes(1, days);
            if (message == "Tesla")
                TakeQuotes(2, days);
            if (message == "AMD")
                TakeQuotes(3, days);
            if (message == "Intel")
                TakeQuotes(4, days);
            if (message == "Amazon")
                TakeQuotes(5, days);
            if (message == "Microsoft")
                TakeQuotes(6, days);
        }
    }
}
