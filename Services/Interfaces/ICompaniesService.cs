using QuotesExchangeApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Services.Interfaces
{
    public interface ICompaniesService
    {
        public Task<IEnumerable<DetaledCompany>> GetCompanies();
    }
}
