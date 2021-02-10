using Microsoft.AspNetCore.Mvc;
using QuotesExchangeApp.Models;
using QuotesExchangeApp.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly ICompaniesService _companiesManager;
        private readonly IQuotesService _quotesService;
        public ChartController(ICompaniesService companiesManager, IQuotesService quotesService)
        {
            _companiesManager = companiesManager;
            _quotesService = quotesService;
        }

        [HttpPost("quotes")]
        public async Task<IEnumerable<Quote>> PostQuotes(IncomingValue incomingValue)
        {
            return await _quotesService.GetQuotes(incomingValue.Name, incomingValue.Min);
        }

        [HttpGet("companies")]
        public async Task<IEnumerable<DetaledCompany>> PostCompanies()
        {
            return await _companiesManager.GetCompanies();
        }
    }
}
