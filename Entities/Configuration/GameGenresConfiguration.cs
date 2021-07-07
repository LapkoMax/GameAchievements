using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration
{
    public class GameGenresConfiguration : IEntityTypeConfiguration<GameGenres>
    {
        public void Configure(EntityTypeBuilder<GameGenres> builder)
        {
            builder.HasData
            (
                new GameGenres
                {
                    Id = 1L,
                    GameId = 1L,
                    GenreId = 1L
                },
                new GameGenres
                {
                    Id = 2L,
                    GameId = 1L,
                    GenreId = 2L
                },
                new GameGenres
                {
                    Id = 3L,
                    GameId = 1L,
                    GenreId = 3L
                },
                new GameGenres
                {
                    Id = 4L,
                    GameId = 1L,
                    GenreId = 4L
                },
                new GameGenres
                {
                    Id = 5L,
                    GameId = 1L,
                    GenreId = 5L
                },
                new GameGenres
                {
                    Id = 6L,
                    GameId = 2L,
                    GenreId = 3L
                },
                new GameGenres
                {
                    Id = 7L,
                    GameId = 2L,
                    GenreId = 4L
                },
                new GameGenres
                {
                    Id = 8L,
                    GameId = 2L,
                    GenreId = 5L
                },
                new GameGenres
                {
                    Id = 9L,
                    GameId = 3L,
                    GenreId = 5L
                },
                new GameGenres
                {
                    Id = 10L,
                    GameId = 3L,
                    GenreId = 6L
                }
            );
        }
    }
}
