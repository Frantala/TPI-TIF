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
        
        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasIndex(user => user.Email)
                .IsUnique();
        }

    }
}