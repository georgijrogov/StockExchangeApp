using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuotesExchangeApp.Data;
using QuotesExchangeApp.Models;
using QuotesExchangeApp.Services.Interfaces;

namespace QuotesExchangeApp.Pages
{
    public class QuoteModel : PageModel
    {
        private readonly IQuotesService _quotesService;
        public IEnumerable<Result> Results { get; set; }

        public QuoteModel(IQuotesService quotesService)
        {
            _quotesService = quotesService;
        }

        public void OnGet()
        {
            Results = _quotesService.GetCompaniesQuotes();
        }
    }
}
