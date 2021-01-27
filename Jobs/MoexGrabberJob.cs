using Microsoft.EntityFrameworkCore;
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
    public class MoexGrabberJob : IJob
    {
        private readonly IConfiguration Configuration;
        private readonly string moexSourceName = "MOEX";
        public List<Company> Companies { get; set; }
        private readonly ApplicationDbContext _context;
        public MoexGrabberJob(ApplicationDbContext db, IConfiguration configuration)
        {
            _context = db;
            Configuration = configuration;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var sourceMOEX = _context.Sources.FirstOrDefault(x => x.Name == moexSourceName);
            var responseRubToUsd = new WebClient().DownloadString("https://www.cbr-xml-daily.ru/latest.js"); //текущий курс рубля в разных валютах
            var moexCompanies = _context.SupportedCompanies.Include(x => x.Company).Where(x => x.Source.Name == moexSourceName).Select(x => x.Company);
            foreach (var company in moexCompanies)
            {
                var response = new WebClient().DownloadString(sourceMOEX.ApiUrl + company.Ticker + ".json");

                dynamic usd = JObject.Parse(responseRubToUsd);
                string usdRate = usd.rates.USD;
                float multiplier = float.Parse(usdRate.Replace(".", ","));

                dynamic moex = JObject.Parse(response);
                string moexstring = moex.marketdata.data[2][12];
                float moexobj = float.Parse(moexstring.Replace(".", ","));
                float cValue = moexobj * multiplier; //Перевод из рублей в доллары
                Quote newquote = new Quote
                {
                    Company = company,
                    Price = (float)Math.Round(cValue, 2),
                    Date = DateTime.Now,
                    Source = sourceMOEX
                };
                _context.Quotes.Add(newquote);
                await Task.Delay(500);
            }
            await _context.SaveChangesAsync();
        }
    }
}
