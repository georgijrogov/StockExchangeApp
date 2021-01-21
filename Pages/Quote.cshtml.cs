using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuotesExchangeApp.Data;
using QuotesExchangeApp.Models;

namespace QuotesExchangeApp.Pages
{
    public class QuoteModel : PageModel
    {

        private readonly ApplicationDbContext _context;
        public List<Quote> Quotes { get; set; }
        public List<Company> Companies { get; set; }
        public List<Result> Results { get; set; }
        public QuoteModel(ApplicationDbContext db)
        {
            _context = db;
        }
        public void OnGet()
        {
            Quotes = _context.Quotes.AsNoTracking().ToList(); //Вывод всех котировок из бд
            //var res = _context.Quotes.FromSqlRaw("SELECT Quotes.Id, Companies.Name, Quotes.Price, Quotes.Date, Sources.Name FROM Quotes JOIN Companies ON Companies.Id = Quotes.Id_Company JOIN Sources ON Sources.Id = Quotes.Id_Source").ToList();
            var res = (from quote in _context.Quotes.Skip(Math.Max(0, Quotes.Count() - 8))
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
        public async Task<IActionResult> OnPostAddToDB()
        {
            string token = "&token=bvu2mc748v6pkq82cr00";
            string apis = "";
            string response = "";
            Quote quote = new Quote();
            List<string> urls = new List<string>();
            List<QuoteResponse> quoteResponses = new List<QuoteResponse>();

            var api = from source in _context.Sources where source.Id == 1 select source.ApiUrl.ToString();
            foreach (var r in api)
                apis = r;
            var comps = from company in _context.Companies where company.Id < 7 select company.Ticker;
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
            api = from source in _context.Sources where source.Id == 2 select source.ApiUrl.ToString();
            foreach (var r in api)
                apis = r;
            comps = from company in _context.Companies where company.Id > 6 select company.Ticker;
            foreach (var r in comps)
            {
                QuoteResponse quoteObj = new QuoteResponse
                {
                    Url = apis + r + ".json",
                    Ticker = r
                };
                quoteResponses.Add(quoteObj);
                urls.Add(apis + r + ".json");
            }

            foreach (var r in quoteResponses)
            {
                if (r.Url.Length <= 70)
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
                    await Task.Delay(500);
                }
                else
                {
                    int i = 0;
                    int tickerID = 1;                    
                    response = new WebClient().DownloadString(r.Url);
                    dynamic moex = JObject.Parse(response);
                    while (i < 6)
                    {
                        if (moex.marketdata.data[i][12] == "0" || moex.marketdata.data[i][12] == null)
                        {
                            i = i + 1;
                        }
                        else
                        {
                            dynamic moexobj = moex.marketdata.data[i][12];
                            float cValue = moexobj;
                            var ticker = from company in _context.Companies where company.Ticker == r.Ticker select company.Id;
                            foreach (var rr in ticker)
                                tickerID = rr;
                            Quote newquote = new Quote
                            {
                                Id_Company = tickerID,
                                Price = cValue,
                                Date = DateTime.Now,
                                Id_Source = 2
                            };
                            _context.Quotes.Add(newquote);
                            await Task.Delay(500);
                            break;
                        }
                    }                                        
                }                
            }
            await _context.SaveChangesAsync();

            return RedirectToPage("Quote");
        }
    }
}
