using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuotesExchangeApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuotesExchangeApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Source> Sources { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Quote>().HasOne(x => x.Company);
            builder.Entity<Quote>().HasOne(x => x.Source);
            builder.Entity<Source>().HasData(
                new Source
                {
                    Name = "Finnhub",
                    ApiUrl = "https://finnhub.io/api/v1/quote?symbol="
                },
                new Source
                {
                    Name = "MOEX",
                    ApiUrl = "https://iss.moex.com/iss/engines/stock/markets/shares/securities/"
                }
                );
            builder.Entity<Company>().HasData(
                new Company
                {
                    Name = "Apple",
                    Ticker = "AAPL"
                },
                new Company
                {
                    Name = "Tesla",
                    Ticker = "TSLA"
                },
                new Company
                {
                    Name = "AMD",
                    Ticker = "AMD"
                },
                new Company
                {
                    Name = "Intel",
                    Ticker = "INTC"
                },
                new Company
                {
                    Name = "Amazon",
                    Ticker = "AMZN"
                },
                new Company
                {
                    Name = "Microsoft",
                    Ticker = "MSFT"
                },
                new Company
                {
                    Name = "Газпром",
                    Ticker = "GAZP"
                },
                new Company
                {
                    Name = "Яндекс",
                    Ticker = "YNDX"
                }
                );
        }
    }
}
