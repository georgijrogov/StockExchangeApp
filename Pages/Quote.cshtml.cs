using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuotesExchangeApp.Models;
using QuotesExchangeApp.Services.Interfaces;

namespace QuotesExchangeApp.Pages
{
    public class QuoteModel : PageModel
    {
        private readonly IQuotesService _quotesService;
        public IEnumerable<DetaledCompany> Results { get; set; }

        public QuoteModel(IQuotesService quotesService)
        {
            _quotesService = quotesService;
        }

        public async Task OnGetAsync()
        {
            Results = await _quotesService.GetCompaniesQuotes();
        }
    }
}
