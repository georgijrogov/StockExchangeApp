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
        public List<Company> Companies { get; set; }
        private readonly string token = "&token=bvu2mc748v6pkq82cr00";
        private string sourceName;
        private readonly ApplicationDbContext _context;
        public DBUpdater(ApplicationDbContext db)
        {
            _context = db;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            sourceName = "Finnhub";
            var source = _context.Sources.FirstOrDefault(x => x.Name == sourceName);
            if (source == null)
                return;
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
            sourceName = "MOEX";
            var source2 = _context.Sources.FirstOrDefault(x => x.Name == sourceName);
            if (source == null)
                return;
            Companies = _context.Companies.ToList();
            var companies2 = _context.Companies.Skip(Math.Max(0, Companies.Count() - 2));
            foreach (var company in companies2)
            {
                int i = 0;
                string response = new WebClient().DownloadString(source2.ApiUrl + company.Ticker + ".json");
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
                        Quote newquote = new Quote
                        {
                            Company = company,
                            Price = cValue,
                            Date = DateTime.Now,
                            Source = source2
                        };
                        _context.Quotes.Add(newquote);
                        await Task.Delay(500);
                        break;
                    }
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
