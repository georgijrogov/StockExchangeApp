using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuotesExchangeApp.Data;
using QuotesExchangeApp.Models;

namespace QuotesExchangeApp.Pages
{
    public class ChartModel : PageModel
    {
        public Dictionary<string, int> TimeSpans { get; set; } = new Dictionary<string, int>()
        {
            { "5 минут", 5 },
            { "1 час", 60 },
            { "4 часа", 240 },
            { "1 день", 1440 },
            { "1 неделя", 10080 },
            { "1 месяц", 43200 },
            { "1 год", 525600 },
            { "Макс.", 10000000 }
        };
        public static Company CurrentCompany { get; set; }
        public List<Result> Results { get; set; }
        public string Json { get; set; }
        private readonly ApplicationDbContext _context;
        public ChartModel(ApplicationDbContext db)
        {
            _context = db;
        }

        public void OnGet()
        {
            CurrentCompany = _context.Companies.FirstOrDefault();
            TakeCompaniesList();
        }

        public void TakeCompaniesList()
        {
            var res = _context.Quotes.Include(x => x.Company).ToList().GroupBy(x => x.Company.Id, (key, g) => g.OrderByDescending(e => e.Date).First());
            Results = (from quote in res.ToList()
                       select new Result
                       {
                           QuoteId = quote.Id,
                           CompanyId = quote.Company.Id,
                           CompanyName = quote.Company.Name,
                           CompanyTicker = quote.Company.Ticker,
                           QuotePrice = quote.Price,
                           QuoteDate = quote.Date
                       }).ToList();
        }
    }
}
