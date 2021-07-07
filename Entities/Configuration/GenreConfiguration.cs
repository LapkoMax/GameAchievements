using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasData
            (
                new Genre
                {
                    Id = 1L,
                    Name = "Dark fantasy",
                    Description = "Combines fantasy with elements of horror or has a gloomy dark tone or a sense of horror and dread."
                },
                new Genre
                {
                    Id = 2L,
                    Name = "Hard game",
                    Description = "Not intended for a casual players."
                },
                new Genre
                {
                    Id = 3L,
                    Name = "RPG",
                    Description = "Players advance through a story quest, and often many side quests, for which their character or party of characters gain experience that improves various attributes and abilities."
                },
                new Genre
                {
                    Id = 4L,
                    Name = "Open world",
                    Description = "Game contains a big open world to explore."
                },
                new Genre
                {
                    Id = 5L,
                    Name = "Action",
                    Description = "Emphasizes physical challenges, including hand–eye coordination and reaction-time."
                },
                new Genre
                {
                    Id = 6L,
                    Name = "Shooter",
                    Description = "Subgenre of action video games where the focus is almost entirely on the defeat of the character's enemies using the weapons given to the player."
                }
            );
        }
    }
}
