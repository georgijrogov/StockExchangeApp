using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuotesExchangeApp.Data;
using QuotesExchangeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public List<Company> Companies { get; set; }
        private readonly ILogger<IndexModel> _logger;
        //WebClient client = new WebClient();
        //public string Json { get; set; } = new WebClient().DownloadString("https://finnhub.io/api/v1/quote?symbol=AAPL&token=bvu2mc748v6pkq82cr00");

        //public IndexModel(ILogger<IndexModel> logger)
        //{
        //    _logger = logger;
        //}
        public IndexModel(ApplicationDbContext db)
        {
            _context = db;
        }

        public void OnGet()
        {
            Companies = _context.Companies.AsNoTracking().ToList();
        }
    }
}
