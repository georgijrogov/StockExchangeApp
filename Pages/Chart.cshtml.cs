using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ApplicationDbContext _context;
        public ChartModel(ApplicationDbContext db)
        {
            _context = db;
        }
        public string Json { get; set; }
        public void OnGet()
        {
            if (CurrentCompany == null)
            {
                CurrentCompany = _context.Companies.FirstOrDefault();
                if (CurrentCompany != null)
                {
                    TakeQuotes(TimeSpans["1 день"], CurrentCompany.Id.ToString());
                }
            }
            TakeCompaniesList();
        }
        public void OnPostMain(string comp)
        {
            TakeQuotes(1440, comp);
        }
        public void OnPostCustom(int min)
        {
            TakeQuotes(min, CurrentCompany.Id.ToString());
        }
        public void TakeQuotes(int d, string c)
        {
            CurrentCompany = _context.Companies.FirstOrDefault(x => x.Id == Guid.Parse(c));
            var res = _context.Quotes.Include(x => x.Company)
                .Where(x => x.Company.Id == Guid.Parse(c) && x.Date > DateTime.Now.AddMinutes(-d)).ToList()
                .OrderBy(x => x.Date)
                .Select(x => new {
                    QuotePrice = x.Price,
                    QuoteDate = x.Date
                }).ToList();
            Json = JsonConvert.SerializeObject(res);
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
                           QuoteDate = quote.Date,
                       }).DistinctBy(x => x.CompanyId).ToList();
        }
    }
}
