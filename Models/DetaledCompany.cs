﻿using System;

namespace QuotesExchangeApp.Models
{
    public class DetaledCompany
    {
        public Guid QuoteId { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyTicker { get; set; }
        public float QuotePrice { get; set; }
        public DateTime QuoteDate { get; set; }
    }
}
