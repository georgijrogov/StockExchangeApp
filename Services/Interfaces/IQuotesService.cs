using QuotesExchangeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Services.Interfaces
{
    public interface IQuotesService
    {
        public string GetQuotes(string companyName, int minSpan);
        public List<Result> GetCompaniesQuotes();
    }
}
