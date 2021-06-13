using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GameAchievements.Models.Entities;
using GameAchievements.Models.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GameAchievements.Models
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new GameConfiguration());
            modelBuilder.ApplyConfiguration(new GenreConfiguration());
            modelBuilder.ApplyConfiguration(new GameGenresConfiguration());
            modelBuilder.ApplyConfiguration(new AchievementConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }
        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
    }
}
