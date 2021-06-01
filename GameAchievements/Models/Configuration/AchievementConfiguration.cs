using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameAchievements.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameAchievements.Models.Configuration
{
    public class AchievementConfiguration : IEntityTypeConfiguration<Achievement>
    {
        public void Configure(EntityTypeBuilder<Achievement> builder)
        {
            builder.HasData
            (
                new Achievement
                {
                    Id = 1L,
                    Name = "Welcome to Dark Souls",
                    Description = "It is just a begining.",
                    Condition = "Die for the first time.",
                    GameId = 1L
                },
                new Achievement
                {
                    Id = 2L,
                    Name = "Judge Gundir",
                    Description = "First boss.",
                    Condition = "Kill the Judge Gundir.",
                    GameId = 1L
                },
                new Achievement
                {
                    Id = 3L,
                    Name = "The work of the witcher",
                    Description = "First monster order.",
                    Condition = "Complete your first order.",
                    GameId = 2L
                },
                new Achievement
                {
                    Id = 4L,
                    Name = "Good ending",
                    Description = "Everything is good.",
                    Condition = "Get the good ending.",
                    GameId = 2L
                },
                new Achievement
                {
                    Id = 5L,
                    Name = "The old friend",
                    Description = "Meet the BFG.",
                    Condition = "Get the BFG.",
                    GameId = 3L
                },
                new Achievement
                {
                    Id = 6L,
                    Name = "IDDQD",
                    Description = "Rune master.",
                    Condition = "Upgrade all runes.",
                    GameId = 3L
                }
            );
        }
    }
}
