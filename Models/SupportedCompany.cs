using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Models
{
    public class SupportedCompany
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Company Company { get; set; }
        public Source Source { get; set; }
    }
}
