using QuotesExchangeApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Services.Interfaces
{
    public interface IQuotesService
    {
        public Task<IEnumerable<Quote>> GetQuotes(string companyName, int minSpan);
        public Task<IEnumerable<DetaledCompany>> GetCompaniesQuotes();
    }
}
