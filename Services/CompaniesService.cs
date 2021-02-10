using Microsoft.EntityFrameworkCore;
using QuotesExchangeApp.Data;
using QuotesExchangeApp.Models;
using QuotesExchangeApp.Services.Interfaces;
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

        public async Task<IEnumerable<DetaledCompany>> GetCompanies()
        {
            var companies = await _context.Quotes.Include(x => x.Company).ToListAsync();
            return companies.GroupBy(x => x.Company.Id, (key, g) => g.OrderByDescending(e => e.Date).First()).Select(quote => 
                new DetaledCompany
                {
                    QuoteId = quote.Id,
                    CompanyId = quote.Company.Id,
                    CompanyName = quote.Company.Name,
                    CompanyTicker = quote.Company.Ticker,
                    QuotePrice = quote.Price,
                    QuoteDate = quote.Date
                });
        }
    }
}
