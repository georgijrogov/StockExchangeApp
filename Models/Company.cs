using System;

namespace QuotesExchangeApp.Models
{
    public class Company
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Ticker { get; set; }
    }
}
