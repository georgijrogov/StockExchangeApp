using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuotesExchangeApp.Data;
using QuotesExchangeApp.Models;

namespace QuotesExchangeApp.Pages
{
    public class QuoteModel : PageModel
    {

        private readonly ApplicationDbContext _context;
        public IEnumerable<Result> Results { get; set; }
        public QuoteModel(ApplicationDbContext db)
        {
            _context = db;
        }
        public void OnGet()
        {
            var res = _context.Quotes.Include(x => x.Company).ToList().GroupBy(x => x.Company.Id, (key, g) => g.OrderByDescending(e => e.Date).First());
            Results = (from quote in res.ToList()
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
