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
        public DbSet<PropertiesObject> PropertiesObjects { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    string adminRoleName = "admin";
        //    string userRoleName = "user";

        //    string adminEmail = "admin@mail.ru";
        //    string adminPassword = "123456";

        //    // добавляем роли
        //    Role adminRole = new Role { Id = 1, Name = adminRoleName };
        //    Role userRole = new Role { Id = 2, Name = userRoleName };
        //    User adminUser = new User { Id = 1, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id };

        //    modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
        //    modelBuilder.Entity<User>().HasData(new User[] { adminUser });
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
