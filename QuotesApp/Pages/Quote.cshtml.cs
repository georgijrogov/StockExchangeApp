using System;
using System.Collections.Generic;
using System.Linq;
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
            Quotes = _context.Quotes.AsNoTracking().ToList();
            //var res = _context.Quotes.FromSqlRaw("SELECT Quotes.Id, Companies.Name, Quotes.Price, Quotes.Date, Sources.Name FROM Quotes JOIN Companies ON Companies.Id = Quotes.Id_Company JOIN Sources ON Sources.Id = Quotes.Id_Source").ToList();
            var res = (from quote in _context.Quotes
                      join company in _context.Companies on quote.Id_Company equals company.Id
                      join source in _context.Sources on quote.Id_Source equals source.Id
                      select new
                      {
                          QuoteID = quote.Id,
                          CompanyName = company.Name,
                          QuotePrice = quote.Price,
                          QuoteDate = quote.Date,
                          SourceName = source.Name,
                      }).ToList();
            //List<string> Results = new List<string>();
            string Json = JsonConvert.SerializeObject(res);
            //Json = Json.Substring(1, Json.Length - 2);
            //Result Results = JsonConvert.DeserializeObject<Result>(Json);
            Results = JsonConvert.DeserializeObject<List<Result>>(Json);
            //JObject Results = JObject.Parse(Json);
            //Results = JsonConvert.DeserializeObject<Result>(json);
            foreach (var r in res)
            {
                //Results[i].Id = r.QuoteID;
                //Results[r.QuoteID].Id = r.QuoteID.ToString();
                //Results[r.QuoteID].CompanyName = r.CompanyName.ToString();
                //Results[r.QuoteID].Price = r.QuotePrice.ToString();
                //Results[r.QuoteID].Date = r.QuoteDate.ToString();
                //Results[r.QuoteID].SourceName = r.SourceName.ToString();
                //Console.WriteLine($"{r.QuoteID} ({r.CompanyName} - {r.QuotePrice}) - {r.QuoteDate} - {r.SourceName}");
            }
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
            string apis = "";
            List<string> urls = new List<string>();
            var api = from source in _context.Sources where source.Id == 1 select source.ApiUrl.ToString();
            foreach (var r in api)
                apis = r;
            var comps = from company in _context.Companies select company.Ticker;
            foreach (var r in comps)
                urls.Add(apis + r);
            var test = 1;
        }
    }
}
