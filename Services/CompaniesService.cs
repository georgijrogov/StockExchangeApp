using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuotesExchangeApp.Data;
using QuotesExchangeApp.Models;
using QuotesExchangeApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Services
{
    public class CompaniesService : ICompaniesService
    {
        private readonly ApplicationDbContext _context;
        public CompaniesService(ApplicationDbContext db)
        {
            _context = db;
        }
        public string GetCompanies()
        {
            var companies = _context.Quotes.Include(x => x.Company).ToList().GroupBy(x => x.Company.Id, (key, g) => g.OrderByDescending(e => e.Date).First());
            var res = (from quote in companies.ToList()
                       select new Result
                       {
                           QuoteId = quote.Id,
                           CompanyId = quote.Company.Id,
                           CompanyName = quote.Company.Name,
                           CompanyTicker = quote.Company.Ticker,
                           QuotePrice = quote.Price,
                           QuoteDate = quote.Date
                       }).ToList();
            return JsonConvert.SerializeObject(res);
        }
    }
}
