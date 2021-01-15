using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuotesApp.Models;

namespace QuotesApp.Pages
{
    public class QuoteModel : PageModel
    {
        private readonly ApplicationContext _context;
        public List<Quote> Quotes { get; set; }
        public List<Company> Companies { get; set; }
        public List<Result> Results { get; set; }
        public QuoteModel(ApplicationContext db)
        {
            _context = db;
        }
        public void OnGet()
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
            //JObject Results = JObject.Parse(Json);
            //Results = JsonConvert.DeserializeObject<Result>(json);
            //foreach (var r in res)
            //{
                //Results[i].Id = r.QuoteID;
                //Results[r.QuoteID].Id = r.QuoteID.ToString();
                //Results[r.QuoteID].CompanyName = r.CompanyName.ToString();
                //Results[r.QuoteID].Price = r.QuotePrice.ToString();
                //Results[r.QuoteID].Date = r.QuoteDate.ToString();
                //Results[r.QuoteID].SourceName = r.SourceName.ToString();
                //Console.WriteLine($"{r.QuoteID} ({r.CompanyName} - {r.QuotePrice}) - {r.QuoteDate} - {r.SourceName}");
            //}
            //ViewData["Res"] = new Result
            //{
                //Id = 1,
                //CompanyName = "adg",
                //Price = 1,
                //Date = new DateTime(2021, 1, 12),
                //SourceName = "asf"
            //};
            //var comps = _context.Companies.FromSqlRaw("SELECT * FROM Companies").ToList();            
        }
        public void OnPostAddToDB()
        {
            string token = "&token=bvu2mc748v6pkq82cr00";
            string apis = "";
            string response ="";
            Quote quote = new Quote();
            List<string> urls = new List<string>();
            List<QuoteResponse> quoteResponses = new List<QuoteResponse>();
            
            var api = from source in _context.Sources where source.Id == 1 select source.ApiUrl.ToString();
            foreach (var r in api)
                apis = r;
            var comps = from company in _context.Companies select company.Ticker;
            foreach (var r in comps)
            {
                QuoteResponse quoteObj = new QuoteResponse
                {
                    Url = apis + r + token,
                    Ticker = r
                };
                quoteResponses.Add(quoteObj);
                urls.Add(apis + r + token);
            }
                
            foreach (var r in quoteResponses)
            {
                int tickerID = 1;
                response = new WebClient().DownloadString(r.Url);
                string c = JObject.Parse(response).SelectToken("c").ToString();
                float cValue = float.Parse(c);
                var ticker = from company in _context.Companies where company.Ticker == r.Ticker select company.Id;
                foreach (var rr in ticker)
                    tickerID = rr;
                Quote newquote = new Quote
                {
                    Id_Company = tickerID,
                    Price = cValue,
                    Date = DateTime.Now,
                    Id_Source = 1
                };
                _context.Quotes.Add(newquote);
                Thread.Sleep(500);
            }
            _context.SaveChanges();
            OnGet();
        }
    }
}
