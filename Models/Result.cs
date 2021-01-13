using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesApp.Models
{
    public class Result
    {
        public int QuoteId { get; set; }
        public string CompanyName { get; set; }
        public float QuotePrice { get; set; }
        public DateTime QuoteDate { get; set; }
        public string SourceName { get; set; }
    }
}
