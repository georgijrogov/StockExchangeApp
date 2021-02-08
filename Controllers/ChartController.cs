using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuotesExchangeApp.Data;
using QuotesExchangeApp.Models;
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
        private readonly ApplicationDbContext _context;
        public ChartController(ApplicationDbContext db)
        {
            _context = db;
        }
        // GET: api/<HomeController>
        [HttpPost]
        public string Post(IncomingValue incomingValue)
        {
            var company = _context.Companies.FirstOrDefault(x => x.Name == incomingValue.Name);
            var res = _context.Quotes.Include(x => x.Company)
                .Where(x => x.Company.Id == company.Id && x.Date > DateTime.Now.AddMinutes(-incomingValue.Min)).ToList()
                .OrderBy(x => x.Date)
                .Select(x => new {
                    QuotePrice = x.Price,
                    QuoteDate = x.Date
                }).ToList();
            return JsonConvert.SerializeObject(res);
        }
    }
}
