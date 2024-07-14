namespace Cadastre.Data
{
    using Cadastre.Data.Models;
    using Microsoft.EntityFrameworkCore;
    public class CadastreContext : DbContext
    {
        public CadastreContext()
        {
            
        }

        public CadastreContext(DbContextOptions options)
            :base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PropertyCitizen>()
                .HasKey(e => new { e.PropertyId, e.CitizenId });

            modelBuilder.Entity<PropertyCitizen>()
                .HasOne(p => p.Property)
                .WithMany(pc => pc.PropertiesCitizens);

            modelBuilder.Entity<PropertyCitizen>()
                .HasOne(c => c.Citizen)
                .WithMany(pc => pc.PropertiesCitizens);

        }

        public DbSet<District> Districts { get; set; } = null!;

        public DbSet<Property> Properties { get; set; } = null!;

        public DbSet<PropertyCitizen> PropertiesCitizens { get; set; } = null!;

        public DbSet<Citizen> Citizens { get; set;} = null!;

    }
}
