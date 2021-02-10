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
        public async Task<IEnumerable<Quote>> GetQuotes(string companyName, int minSpan)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(x => x.Name == companyName);
            return _context.Quotes.Include(x => x.Company)
                .Where(x => x.Company.Id == company.Id && x.Date > DateTime.Now.AddMinutes(-minSpan)).ToList()
                .OrderBy(x => x.Date)
                .Select(x => new Quote
                {
                    Price = x.Price,
                    Date = x.Date
                });
        }
        public async Task<IEnumerable<DetaledCompany>> GetCompaniesQuotes()
        {
            var res = await _context.Quotes.Include(x => x.Company).ToListAsync();
                return res.GroupBy(x => x.Company.Id, (key, g) => g.OrderByDescending(e => e.Date).First()).Select(quote =>
                    new DetaledCompany
                    {
                        QuoteId = quote.Id,
                        CompanyId = quote.Company.Id,
                        CompanyName = quote.Company.Name,
                        CompanyTicker = quote.Company.Ticker,
                        QuotePrice = quote.Price,
                        QuoteDate = quote.Date,
                    });
        }
    }
}
