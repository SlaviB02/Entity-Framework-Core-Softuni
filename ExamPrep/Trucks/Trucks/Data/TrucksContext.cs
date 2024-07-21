namespace Trucks.Data
{
    using Microsoft.EntityFrameworkCore;
    using Trucks.Data.Models;

    public class TrucksContext : DbContext
    {
        public TrucksContext()
        { 
        }

        public TrucksContext(DbContextOptions options)
            : base(options) 
        { 
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ClientTruck>()
                .HasKey(e => new { e.TruckId, e.ClientId });


            modelBuilder.Entity<ClientTruck>()
                .HasOne(c => c.Client)
                .WithMany(ct => ct.ClientsTrucks);

            modelBuilder.Entity<ClientTruck>()
                .HasOne(t => t.Truck)
                .WithMany(ct => ct.ClientsTrucks);


        }

        public DbSet<Truck> Trucks { get; set; } = null!;

        public DbSet<Client> Clients { get; set; } = null!;

        public DbSet<Despatcher> Despatchers { get; set;} = null!;

        public DbSet<ClientTruck> ClientsTrucks { get; set; }=null!;
    }
}
