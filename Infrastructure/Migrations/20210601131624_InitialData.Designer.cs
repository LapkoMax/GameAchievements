﻿// <auto-generated />
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20210601131624_InitialData")]
    partial class InitialData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GameAchievements.Models.Entities.Achievement", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("AchievementId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Condition")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<long>("GameId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Achievements");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Condition = "Die for the first time.",
                            Description = "It is just a begining.",
                            GameId = 1L,
                            Name = "Welcome to Dark Souls"
                        },
                        new
                        {
                            Id = 2L,
                            Condition = "Kill the Judge Gundir.",
                            Description = "First boss.",
                            GameId = 1L,
                            Name = "Judge Gundir"
                        },
                        new
                        {
                            Id = 3L,
                            Condition = "Complete your first order.",
                            Description = "First monster order.",
                            GameId = 2L,
                            Name = "The work of the witcher"
                        },
                        new
                        {
                            Id = 4L,
                            Condition = "Get the good ending.",
                            Description = "Everything is good.",
                            GameId = 2L,
                            Name = "Good ending"
                        },
                        new
                        {
                            Id = 5L,
                            Condition = "Get the BFG.",
                            Description = "Meet the BFG.",
                            GameId = 3L,
                            Name = "The old friend"
                        },
                        new
                        {
                            Id = 6L,
                            Condition = "Upgrade all runes.",
                            Description = "Rune master.",
                            GameId = 3L,
                            Name = "IDDQD"
                        });
                });

            modelBuilder.Entity("GameAchievements.Models.Entities.Game", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("GameId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Games");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Description = "Hardcore dark fantasy.",
                            Name = "Dark Souls",
                            Rating = 9.3000000000000007
                        },
                        new
                        {
                            Id = 2L,
                            Description = "Fantasy action about monster slayer.",
                            Name = "The Witcher",
                            Rating = 9.6999999999999993
                        },
                        new
                        {
                            Id = 3L,
                            Description = "Fast shooter where you can take out your anger on demons",
                            Name = "DOOM",
                            Rating = 9.4000000000000004
                        });
                });

            modelBuilder.Entity("GameAchievements.Models.Entities.GameGenres", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("GameId")
                        .HasColumnType("bigint");

                    b.Property<long>("GenreId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("GenreId");

                    b.ToTable("GameGenres");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            GameId = 1L,
                            GenreId = 1L
                        },
                        new
                        {
                            Id = 2L,
                            GameId = 1L,
                            GenreId = 2L
                        },
                        new
                        {
                            Id = 3L,
                            GameId = 1L,
                            GenreId = 3L
                        },
                        new
                        {
                            Id = 4L,
                            GameId = 1L,
                            GenreId = 4L
                        },
                        new
                        {
                            Id = 5L,
                            GameId = 1L,
                            GenreId = 5L
                        },
                        new
                        {
                            Id = 6L,
                            GameId = 2L,
                            GenreId = 3L
                        },
                        new
                        {
                            Id = 7L,
                            GameId = 2L,
                            GenreId = 4L
                        },
                        new
                        {
                            Id = 8L,
                            GameId = 2L,
                            GenreId = 5L
                        },
                        new
                        {
                            Id = 9L,
                            GameId = 3L,
                            GenreId = 5L
                        },
                        new
                        {
                            Id = 10L,
                            GameId = 3L,
                            GenreId = 6L
                        });
                });

            modelBuilder.Entity("GameAchievements.Models.Entities.Genre", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("GenreId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.ToTable("Genres");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Description = "Combines fantasy with elements of horror or has a gloomy dark tone or a sense of horror and dread.",
                            Name = "Dark fantasy"
                        },
                        new
                        {
                            Id = 2L,
                            Description = "Not intended for a casual players.",
                            Name = "Hard game"
                        },
                        new
                        {
                            Id = 3L,
                            Description = "Players advance through a story quest, and often many side quests, for which their character or party of characters gain experience that improves various attributes and abilities.",
                            Name = "RPG"
                        },
                        new
                        {
                            Id = 4L,
                            Description = "Game contains a big open world to explore.",
                            Name = "Open world"
                        },
                        new
                        {
                            Id = 5L,
                            Description = "Emphasizes physical challenges, including hand–eye coordination and reaction-time.",
                            Name = "Action"
                        },
                        new
                        {
                            Id = 6L,
                            Description = "Subgenre of action video games where the focus is almost entirely on the defeat of the character's enemies using the weapons given to the player.",
                            Name = "Shooter"
                        });
                });

            modelBuilder.Entity("GameAchievements.Models.Entities.Achievement", b =>
                {
                    b.HasOne("GameAchievements.Models.Entities.Game", "Game")
                        .WithMany("Achievements")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("GameAchievements.Models.Entities.GameGenres", b =>
                {
                    b.HasOne("GameAchievements.Models.Entities.Game", "Game")
                        .WithMany("Genres")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameAchievements.Models.Entities.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("GameAchievements.Models.Entities.Game", b =>
                {
                    b.Navigation("Achievements");

                    b.Navigation("Genres");
                });
#pragma warning restore 612, 618
        }
    }
}