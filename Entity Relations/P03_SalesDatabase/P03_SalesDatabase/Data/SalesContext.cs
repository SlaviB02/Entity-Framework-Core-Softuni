using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P03_SalesDatabase.Data
{
    public class SalesContext:DbContext
    {
        public SalesContext()
        {
            
        }
        public SalesContext(DbContextOptions options):base(options) 
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=SalesDatabase;Integrated Security=True;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {




            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Customer> Customers { get; set; } = null!;

        public DbSet<Product> Products { get; set; } = null!;

        public DbSet<Sale> Sales { get; set; } = null!;

        public DbSet<Store> Stores { get; set; } = null!;
    }
}
