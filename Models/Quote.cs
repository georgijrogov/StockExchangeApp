using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Models
{
    public class Quote
    {
        public int Id { get; set; }
        public int Id_Company { get; set; }
        public float Price { get; set; }
        public DateTime Date { get; set; }
        public int Id_Source { get; set; }
    }
}
