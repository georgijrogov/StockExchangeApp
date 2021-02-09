using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuotesExchangeApp.Data;
using QuotesExchangeApp.Models;
using QuotesExchangeApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        // GET: api/<HomeController>
        [HttpPost("quotes")]
        public string PostQuotes(IncomingValue incomingValue)
        {
            return _quotesService.GetQuotes(incomingValue.Name, incomingValue.Min);
        }
        [HttpGet("companies")]
        public string PostCompanies()
        {
            return _companiesManager.GetCompanies();
        }
    }
}
