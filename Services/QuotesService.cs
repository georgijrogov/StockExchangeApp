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
    public class QuotesService : IQuotesService
    {
        private readonly ApplicationDbContext _context;
        public QuotesService(ApplicationDbContext db)
        {
            _context = db;
        }
        public string GetQuotes(string companyName, int minSpan)
        {
            var company = _context.Companies.FirstOrDefault(x => x.Name == companyName);
            var res = _context.Quotes.Include(x => x.Company)
                .Where(x => x.Company.Id == company.Id && x.Date > DateTime.Now.AddMinutes(-minSpan)).ToList()
                .OrderBy(x => x.Date)
                .Select(x => new {
                    QuotePrice = x.Price,
                    QuoteDate = x.Date
                }).ToList();
            return JsonConvert.SerializeObject(res);
        }
        public List<Result> GetCompaniesQuotes()
        {
            var res = _context.Quotes.Include(x => x.Company).ToList().GroupBy(x => x.Company.Id, (key, g) => g.OrderByDescending(e => e.Date).First());
            return (from quote in res.ToList()
                       select new Result
                       {
                           QuoteId = quote.Id,
                           CompanyId = quote.Company.Id,
                           CompanyName = quote.Company.Name,
                           CompanyTicker = quote.Company.Ticker,
                           QuotePrice = quote.Price,
                           QuoteDate = quote.Date,
                       }).ToList();
        }
    }
}
