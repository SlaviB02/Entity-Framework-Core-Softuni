using Microsoft.EntityFrameworkCore;
using P02_FootballBetting.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data
{
    public class FootballBettingContext:DbContext
    {
        public FootballBettingContext()
        {
            
        }
        public FootballBettingContext(DbContextOptions options)
            : base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerStatistic>()
                .HasKey(e => new { e.PlayerId, e.GameId });

           

            modelBuilder.Entity<Team>()
               .HasOne(e => e.SecondaryKitColor)
               .WithMany(e => e.SecondaryKitTeams)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Game>()
                .HasOne(e=>e.HomeTeam)
                .WithMany(e=>e.HomeGames)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Player>()
                .HasOne(e=>e.Town)
                .WithMany(e=>e.Players)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PlayerStatistic>()
                .HasOne(e=>e.Player)
                .WithMany(e=>e.PlayersStatistics)
                .OnDelete(DeleteBehavior.NoAction);


            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(optionsBuilder.IsConfigured==false)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=FootballBookmakerSystem;Integrated Security=True;");
            }
        }

        public DbSet<Bet> Bets { get; set; } = null!;
        public DbSet<Color> Colors { get; set; } = null!;

        public DbSet<Country> Countries { get; set; } = null!;

        public DbSet<Game> Games { get; set; } = null!;

        public DbSet<Player> Players { get; set; } = null!;

        public DbSet<PlayerStatistic> PlayerStatistics = null!;
        
        public DbSet<Position> Positions { get; set; } = null!;

        public DbSet<Team> Teams { get; set; } = null!;

        public DbSet<Town> Towns { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<PlayerStatistic> PlayersStatistics { get; set; } = null!;
    }
}
