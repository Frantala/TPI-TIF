using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistance
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Team> Teams{ get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!; 
        public DbSet<Match> Matches { get; set; } = null!; 
        public DbSet<Favourite> Favourites { get; set; } = null!; 
        public DbSet<Competition> Competitions { get; set; } = null!; 
        public DbSet<CompetitionTeam> CompetitionTeams { get; set; } = null!;
        
        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasIndex(user => user.Email)
                .IsUnique();

            modelBuilder.Entity<Favourite>()
        .HasOne(f => f.User)
        .WithMany()
        .HasForeignKey(f => f.UserId);

        modelBuilder.Entity<Favourite>()
            .HasOne(f => f.Team)
            .WithMany()
            .HasForeignKey(f => f.TeamId);

        modelBuilder.Entity<Favourite>()
            .HasIndex(f => new { f.UserId, f.TeamId })
            .IsUnique();

                // Relación Match - Competition
        modelBuilder.Entity<Match>()
            .HasOne(m => m.Competition)
            .WithMany()
            .HasForeignKey(m => m.CompetitionId);

        // Relación Match - HomeTeam
        modelBuilder.Entity<Match>()
            .HasOne(m => m.HomeTeam)
            .WithMany()
            .HasForeignKey(m => m.HomeTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación Match - AwayTeam
        modelBuilder.Entity<Match>()
            .HasOne(m => m.AwayTeam)
            .WithMany()
            .HasForeignKey(m => m.AwayTeamId)
            .OnDelete(DeleteBehavior.Restrict);


        // Relación CompetitionTeam - Competition
        modelBuilder.Entity<CompetitionTeam>()
            .HasOne(ct => ct.Competition)
            .WithMany()
            .HasForeignKey(ct => ct.CompetitionId);

        // Relación CompetitionTeam - Team
        modelBuilder.Entity<CompetitionTeam>()
            .HasOne(ct => ct.Team)
            .WithMany()
            .HasForeignKey(ct => ct.TeamId);

        // Evitar relaciones duplicadas para una misma temporada
        modelBuilder.Entity<CompetitionTeam>()
            .HasIndex(ct => new
            {
                ct.CompetitionId,
                ct.TeamId,
                ct.Season
            })
            .IsUnique();
        }

    }
}