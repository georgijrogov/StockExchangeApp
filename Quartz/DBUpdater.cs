using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration Configuration;
        public List<Company> Companies { get; set; }
        private readonly ApplicationDbContext _context;
        public DBUpdater(ApplicationDbContext db, IConfiguration configuration)
        {
            _context = db;
            Configuration = configuration;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var sourceFinnhub = _context.Sources.FirstOrDefault(x => x.Name == "Finnhub");
            if (sourceFinnhub == null)
                return;
            var sourceMOEX = _context.Sources.FirstOrDefault(x => x.Name == "MOEX");
            if (sourceMOEX == null)
                return;
            var companies = _context.Companies.ToList();
            foreach (var company in companies)
            {
                string response = new WebClient().DownloadString(sourceFinnhub.ApiUrl + company.Ticker + Configuration["FinnhubToken"]);
                string price = JObject.Parse(response).SelectToken("c").ToString();
                float cValue = float.Parse(price);
                if (cValue != 0)
                {
                    Quote newquote = new Quote
                    {
                        Company = company,
                        Price = cValue,
                        Date = DateTime.Now,
                        Source = sourceFinnhub
                    };
                    _context.Quotes.Add(newquote);
                    await Task.Delay(500);
                }
                else
                {
                    int i = 0;
                    response = new WebClient().DownloadString(sourceMOEX.ApiUrl + company.Ticker + ".json");
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
                            cValue = moexobj;
                            Quote newquote = new Quote
                            {
                                Company = company,
                                Price = cValue,
                                Date = DateTime.Now,
                                Source = sourceMOEX
                            };
                            _context.Quotes.Add(newquote);
                            await Task.Delay(500);
                            break;
                        }
                    }
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
