﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Quartz;
using QuotesExchangeApp.Controllers;
using QuotesExchangeApp.Data;
using QuotesExchangeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Jobs
{
    public class FinnhubGrabberJob : IJob
    {
        public List<Company> Companies { get; set; }
        private readonly string finnhubSourceName = "Finnhub";
        private readonly IConfiguration Configuration;
        IHubContext<ChartHub> hubContext;
        private readonly ApplicationDbContext _context;
        public FinnhubGrabberJob(ApplicationDbContext db, IConfiguration configuration, IHubContext<ChartHub> hubContext)
        {
            _context = db;
            Configuration = configuration;
            this.hubContext = hubContext;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var sourceFinnhub = _context.Sources.FirstOrDefault(x => x.Name == finnhubSourceName);
            var finnhubCompanies = _context.SupportedCompanies.Include(x => x.Company).Where(x => x.Source.Name == finnhubSourceName).Select(x => x.Company);
            foreach (var company in finnhubCompanies)
            {
                string response = new WebClient().DownloadString(sourceFinnhub.ApiUrl + company.Ticker + Configuration["FinnhubToken"]);
                string rawPrice = JObject.Parse(response).SelectToken("c").ToString();

                Quote newquote = new Quote
                {
                    Company = company,
                    Price = float.Parse(rawPrice),
                    Date = DateTime.Now,
                    Source = sourceFinnhub
                };

                _context.Quotes.Add(newquote);
                await Task.Delay(500);
            }
            await _context.SaveChangesAsync();
            await hubContext.Clients.All.SendAsync("Notify", $"Добавлено: - {DateTime.Now.ToShortTimeString()}");
        }
    }
}
