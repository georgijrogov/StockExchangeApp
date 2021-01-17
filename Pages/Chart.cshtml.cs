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
        public List<Quote> Quotes { get; set; }
        public List<Result> Results { get; set; }
        public string Message { get; set; }
        public string Comp { get; set; }
        private readonly ApplicationDbContext _context;
        public ChartModel(ApplicationDbContext db)
        {
            _context = db;
        }
        public string Json { get; set; }
        public void OnGet()
        {
            Message = "AAPL";
            Comp = "Apple";
            TakeQuotes(1, 1);
            TakeCompaniesList();
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
            TakeCompaniesList();
        }
        public void TakeCompaniesList()
        {
            Quotes = _context.Quotes.AsNoTracking().ToList(); //Вывод всех котировок из бд
            //var res = _context.Quotes.FromSqlRaw("SELECT Quotes.Id, Companies.Name, Quotes.Price, Quotes.Date, Sources.Name FROM Quotes JOIN Companies ON Companies.Id = Quotes.Id_Company JOIN Sources ON Sources.Id = Quotes.Id_Source").ToList();
            var res = (from quote in _context.Quotes.Skip(Math.Max(0, Quotes.Count() - 6))
                       join company in _context.Companies on quote.Id_Company equals company.Id
                       join source in _context.Sources on quote.Id_Source equals source.Id
                       select new
                       {
                           QuoteID = quote.Id,
                           CompanyName = company.Name,
                           CompanyTicker = company.Ticker,
                           QuotePrice = quote.Price,
                           QuoteDate = quote.Date,
                       }).ToList();
            //List<string> Results = new List<string>();
            string Json = JsonConvert.SerializeObject(res);
            //Json = Json.Substring(1, Json.Length - 2);
            //Result Results = JsonConvert.DeserializeObject<Result>(Json);
            Results = JsonConvert.DeserializeObject<List<Result>>(Json);
        }
        public void SetValues(int days, string message)
        {
            Message = message;
            if (message == "AAPL")
            {
                Comp = "Apple";
                TakeQuotes(1, days);
            }
            if (message == "TSLA")
            {
                Comp = "Tesla";
                TakeQuotes(2, days);
            }
            if (message == "AMD")
            {
                Comp = "AMD";
                TakeQuotes(3, days);
            }
            if (message == "INTC")
            {
                Comp = "Intel";
                TakeQuotes(4, days);
            }
            if (message == "AMZN")
            {
                Comp = "Amazon";
                TakeQuotes(5, days);
            }
            if (message == "MSFT")
            {
                Comp = "Microsoft";
                TakeQuotes(6, days);
            }
        }
    }
}
