using System;

namespace QuotesExchangeApp.Models
{
    public class Source
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string ApiUrl { get; set; }
    }
}
