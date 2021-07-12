using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class InitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "GameId", "Description", "Name", "Rating" },
                values: new object[,]
                {
                    { 1L, "Hardcore dark fantasy.", "Dark Souls", 9.3000000000000007 },
                    { 2L, "Fantasy action about monster slayer.", "The Witcher", 9.6999999999999993 },
                    { 3L, "Fast shooter where you can take out your anger on demons", "DOOM", 9.4000000000000004 }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "GenreId", "Description", "Name" },
                values: new object[,]
                {
                    { 1L, "Combines fantasy with elements of horror or has a gloomy dark tone or a sense of horror and dread.", "Dark fantasy" },
                    { 2L, "Not intended for a casual players.", "Hard game" },
                    { 3L, "Players advance through a story quest, and often many side quests, for which their character or party of characters gain experience that improves various attributes and abilities.", "RPG" },
                    { 4L, "Game contains a big open world to explore.", "Open world" },
                    { 5L, "Emphasizes physical challenges, including hand–eye coordination and reaction-time.", "Action" },
                    { 6L, "Subgenre of action video games where the focus is almost entirely on the defeat of the character's enemies using the weapons given to the player.", "Shooter" }
                });

            migrationBuilder.InsertData(
                table: "Achievements",
                columns: new[] { "AchievementId", "Condition", "Description", "GameId", "Name" },
                values: new object[,]
                {
                    { 1L, "Die for the first time.", "It is just a begining.", 1L, "Welcome to Dark Souls" },
                    { 2L, "Kill the Judge Gundir.", "First boss.", 1L, "Judge Gundir" },
                    { 3L, "Complete your first order.", "First monster order.", 2L, "The work of the witcher" },
                    { 4L, "Get the good ending.", "Everything is good.", 2L, "Good ending" },
                    { 5L, "Get the BFG.", "Meet the BFG.", 3L, "The old friend" },
                    { 6L, "Upgrade all runes.", "Rune master.", 3L, "IDDQD" }
                });

            migrationBuilder.InsertData(
                table: "GameGenres",
                columns: new[] { "Id", "GameId", "GenreId" },
                values: new object[,]
                {
                    { 1L, 1L, 1L },
                    { 2L, 1L, 2L },
                    { 3L, 1L, 3L },
                    { 6L, 2L, 3L },
                    { 4L, 1L, 4L },
                    { 7L, 2L, 4L },
                    { 5L, 1L, 5L },
                    { 8L, 2L, 5L },
                    { 9L, 3L, 5L },
                    { 10L, 3L, 6L }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "GameGenres",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 6L);
        }
    }
}
