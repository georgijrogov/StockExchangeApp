using Newtonsoft.Json.Linq;
using Quartz;
using QuotesExchangeApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Models
{
    public class DBUpdater : IJob
    {
        private readonly string token = "&token=bvu2mc748v6pkq82cr00";
        private readonly ApplicationDbContext _context;
        public DBUpdater(ApplicationDbContext db)
        {
            _context = db;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var source = _context.Sources.FirstOrDefault();
            var companies = _context.Companies.Take(6);
            foreach (var company in companies)
            {
                string response = new WebClient().DownloadString(source.ApiUrl + company.Ticker + token);
                string price = JObject.Parse(response).SelectToken("c").ToString();
                float cValue = float.Parse(price);
                Quote newquote = new Quote
                {
                    Company = company,
                    Price = cValue,
                    Date = DateTime.Now,
                    Source = source
                };
                _context.Quotes.Add(newquote);
                await Task.Delay(500);
            }
            await _context.SaveChangesAsync();
        }
        private void RunAnotherSource()
        {
            //api = from source in _context.Sources where source.Id == 2 select source.ApiUrl.ToString();
            //foreach (var r in api)
            //    apis = r;
            //comps = from company in _context.Companies where company.Id > 6 select company.Ticker;
            //foreach (var r in comps)
            //{
            //    QuoteResponse quoteObj = new QuoteResponse
            //    {
            //        Url = apis + r + ".json",
            //        Ticker = r
            //    };
            //    quoteResponses.Add(quoteObj);
            //    urls.Add(apis + r + ".json");
            //}

            //foreach (var r in quoteResponses)
            //{
            //    int i = 0;
            //    int tickerID = 1;
            //    response = new WebClient().DownloadString(r.Url);
            //    dynamic moex = JObject.Parse(response);
            //    while (i < 6)
            //    {
            //        if (moex.marketdata.data[i][12] == "0" || moex.marketdata.data[i][12] == null)
            //        {
            //            i = i + 1;
            //        }
            //        else
            //        {
            //            dynamic moexobj = moex.marketdata.data[i][12];
            //            float cValue = moexobj;
            //            var ticker = from company in _context.Companies where company.Ticker == r.Ticker select company.Id;
            //            foreach (var rr in ticker)
            //                tickerID = rr;
            //            Quote newquote = new Quote
            //            {
            //                Id_Company = tickerID,
            //                Price = cValue,
            //                Date = DateTime.Now,
            //                Id_Source = 2
            //            };
            //            _context.Quotes.Add(newquote);
            //            await Task.Delay(500);
            //            break;
            //        }
            //    }

            //}
        }
    }
}
