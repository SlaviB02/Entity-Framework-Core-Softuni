﻿namespace Footballers.Data
{
    using Footballers.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class FootballersContext : DbContext
    {
        public FootballersContext() { }

        public FootballersContext(DbContextOptions options)
            : base(options) { }


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
            modelBuilder.Entity<TeamFootballer>()
                .HasKey(e => new { e.TeamId, e.FootballerId });

            modelBuilder.Entity<TeamFootballer>()
                .HasOne(t => t.Team)
                .WithMany(tf => tf.TeamsFootballers);

            modelBuilder.Entity<TeamFootballer>()
                .HasOne(f => f.Footballer)
                .WithMany(tf => tf.TeamsFootballers);
        }

        public DbSet<Coach> Coaches { get; set; } = null!;

        public DbSet<Team> Teams { get; set; } = null!;

        public DbSet<Footballer> Footballers { get; set;} = null!;

        public DbSet<TeamFootballer> TeamsFootballers { get;set; } = null!;
    }
}
