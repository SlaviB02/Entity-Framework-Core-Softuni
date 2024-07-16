namespace Boardgames.Data
{
    using Boardgames.Data.Models;
    using Microsoft.EntityFrameworkCore;
    
    public class BoardgamesContext : DbContext
    {
        public BoardgamesContext()
        { 
        }

        public BoardgamesContext(DbContextOptions options)
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

            modelBuilder.Entity<BoardgameSeller>()
                .HasKey(e => new { e.SellerId, e.BoardgameId });

            modelBuilder.Entity<BoardgameSeller>()
                .HasOne(b => b.Boardgame)
                .WithMany(bs => bs.BoardgamesSellers);

            modelBuilder.Entity<BoardgameSeller>()
                .HasOne(s => s.Seller)
                .WithMany(bs => bs.BoardgamesSellers);

        }

        public DbSet<Creator> Creators { get; set; } = null!;

        public DbSet<Boardgame> Boardgames { get; set; } = null!;

        public DbSet<BoardgameSeller> BoardgamesSellers { get;set; } = null!;

        public DbSet<Seller> Sellers { get; set; } = null!;
    }
}
