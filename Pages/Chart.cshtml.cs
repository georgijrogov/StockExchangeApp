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
            //TakeQuotes(1, 1440);
            TakeCompaniesList();
        }
        public void OnPost(string comp)
        {
            TakeQuotes(1440, comp);
        }
        public void OnPostFiveMin(string comp)
        {
            TakeQuotes(5, comp);
        }
        public void OnPostHour(string comp)
        {
            TakeQuotes(60, comp);
        }
        public void OnPostFourHours(string comp)
        {
            TakeQuotes(240, comp);
        }
        public void OnPostDay(string comp)
        {
            TakeQuotes(1440, comp);
        }
        public void OnPostWeek(string comp)
        {
            TakeQuotes(10080, comp);
        }
        public void OnPostMonth(string comp)
        {
            TakeQuotes(43200, comp);
        }
        public void OnPostYear(string comp)
        {
            TakeQuotes(525600, comp);
        }
        public void OnPostMax(string comp)
        {
            TakeQuotes(10000000, comp);
        }
        public void TakeQuotes(int d, string c)
        {
            var test = DateTime.Now.AddMinutes(-d);
            //var res = (from quote in _context.Quotes
            //           where quote.Id_Company == c && quote.Date > DateTime.Now.AddMinutes(-d)
            //           orderby quote.Date
            //           select new
            //           {
            //               QuotePrice = quote.Price,
            //               QuoteDate = quote.Date
            //           }).ToList();
            var res = _context.Quotes.Include(x => x.Company)
                .Where(x => x.Company.Id.ToString() == c && x.Date > DateTime.Now.AddMinutes(-d)).ToList()
                .OrderBy(x => x.Date)
                .Select(x => new {
                    CompanyTicker = x.Company.Ticker,
                    CompanyName = x.Company.Name,
                    QuotePrice = x.Price,
                    QuoteDate = x.Date
                }).ToList();
            var firstQuote = res.FirstOrDefault();
            if (firstQuote != null)
            {
                Comp = firstQuote.CompanyName;
            }
            Json = JsonConvert.SerializeObject(res);
            TakeCompaniesList();
        }
        public void TakeCompaniesList()
        {
            Quotes = _context.Quotes.AsNoTracking().ToList(); //Вывод всех котировок из бд
            //var res = (from quote in _context.Quotes.Skip(Math.Max(0, Quotes.Count() - 8))
            //           join company in _context.Companies on quote.Id_Company equals company.Id
            //           join source in _context.Sources on quote.Id_Source equals source.Id
            //           select new
            //           {
            //               QuoteID = quote.Id,
            //               CompanyName = company.Name,
            //               CompanyTicker = company.Ticker,
            //               QuotePrice = quote.Price,
            //               QuoteDate = quote.Date,
            //           }).ToList();
            Results = (from quote in _context.Quotes.Include(x => x.Company).Skip(Math.Max(0, Quotes.Count() - 8))
                       select new Result
                       {
                           QuoteId = quote.Id,
                           CompanyId = quote.Company.Id,
                           CompanyName = quote.Company.Name,
                           CompanyTicker = quote.Company.Ticker,
                           QuotePrice = quote.Price,
                           QuoteDate = quote.Date,
                       }).ToList();
        }
        
        //public void SetValues(int min, string companyId)
        //{
        //    Message = message;
        //    if (message == "AAPL")
        //    {
        //        Comp = "Apple";
        //        TakeQuotes(1, days);
        //    }
        //    if (message == "TSLA")
        //    {
        //        Comp = "Tesla";
        //        TakeQuotes(2, days);
        //    }
        //    if (message == "AMD")
        //    {
        //        Comp = "AMD";
        //        TakeQuotes(3, days);
        //    }
        //    if (message == "INTC")
        //    {
        //        Comp = "Intel";
        //        TakeQuotes(4, days);
        //    }
        //    if (message == "AMZN")
        //    {
        //        Comp = "Amazon";
        //        TakeQuotes(5, days);
        //    }
        //    if (message == "MSFT")
        //    {
        //        Comp = "Microsoft";
        //        TakeQuotes(6, days);
        //    }
        //    if (message == "GAZP")
        //    {
        //        Comp = "Газпром";
        //        TakeQuotes(7, days);
        //    }
        //    if (message == "YNDX")
        //    {
        //        Comp = "Яндекс";
        //        TakeQuotes(8, days);
        //    }
        //}
    }
}
