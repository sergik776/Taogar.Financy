using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taogar.Finance.Domain.Models;

namespace Taogar.Finance.Infrastructure
{
    public class FinancyDBContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public FinancyDBContext(DbContextOptions<FinancyDBContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Taogar_Financy_DB.db");
            optionsBuilder.UseTriggers(triggerOptions =>
            {
                //triggerOptions.AddTrigger<NoteTrigger>();
            });
            base.OnConfiguring(optionsBuilder);
        }

        public void InitializeDatabase()
        {
            if (Database.EnsureCreated())
            {
                Seed();
            }
        }

        private void Seed()
        {
            
            SaveChanges();
        }

    }
}
