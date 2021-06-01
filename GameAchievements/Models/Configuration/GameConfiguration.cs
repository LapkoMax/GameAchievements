using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameAchievements.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameAchievements.Models.Configuration
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasData
            (
                new Game
                {
                    Id = 1L,
                    Name = "Dark Souls",
                    Description = "Hardcore dark fantasy.",
                    Rating = 9.3
                },
                new Game
                {
                    Id = 2L,
                    Name = "The Witcher",
                    Description = "Fantasy action about monster slayer.",
                    Rating = 9.7
                },
                new Game
                {
                    Id = 3L,
                    Name = "DOOM",
                    Description = "Fast shooter where you can take out your anger on demons",
                    Rating = 9.4
                }
            );
        }
    }
}
