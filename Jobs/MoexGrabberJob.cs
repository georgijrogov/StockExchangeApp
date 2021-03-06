﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Quartz;
using QuotesExchangeApp.Data;
using QuotesExchangeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Jobs
{
    public class MoexGrabberJob : IJob
    {
        private readonly string moexSourceName = "MOEX";
        public List<Company> Companies { get; set; }
        private readonly ApplicationDbContext _context;
        public MoexGrabberJob(ApplicationDbContext db)
        {
            _context = db;
        }

        private float GetCurrencyMultiplier()
        {
            var usdRateJson = new WebClient().DownloadString("https://www.cbr-xml-daily.ru/latest.js"); //Текущий курс рубля в разных валютах

            dynamic usd = JObject.Parse(usdRateJson);
            string usdRate = usd.rates.USD;
            float multiplier = float.Parse(usdRate.Replace(".", ","));

            return multiplier;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            float multiplier = GetCurrencyMultiplier();
            var sourceMOEX = await _context.Sources.FirstOrDefaultAsync(x => x.Name == moexSourceName);
            var moexCompanies = _context.SupportedCompanies.Include(x => x.Company).Where(x => x.Source.Name == moexSourceName).Select(x => x.Company);
            foreach (var company in moexCompanies)
            {
                var response = new WebClient().DownloadString(sourceMOEX.ApiUrl + company.Ticker + ".json");

                if (response != null)
                {
                    dynamic moex = JObject.Parse(response);
                    string moexstring = moex.marketdata.data[2][12]; //В этом значении находится текущая цена акции
                    float rawPrice = float.Parse(moexstring.Replace(".", ","));
                    float price = rawPrice * multiplier; //Перевод из рублей в доллары

                    Quote newquote = new Quote
                    {
                        Company = company,
                        Price = (float)Math.Round(price, 2),
                        Date = DateTime.Now,
                        Source = sourceMOEX
                    };

                    _context.Quotes.Add(newquote);
                }    
                
                await Task.Delay(500); //Задержка между запросами
            }

            await _context.SaveChangesAsync();
        }
    }
}
