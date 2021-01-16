using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace QuotesApp.Models
{
    public class DBUpdater : IJob
    {
        private readonly ApplicationContext _context;
        public DBUpdater(ApplicationContext db)
        {
            _context = db;
        }
        public async Task Execute(IJobExecutionContext context)
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
                await Task.Delay(500);
            }
            await _context.SaveChangesAsync();
        }
    }
}
