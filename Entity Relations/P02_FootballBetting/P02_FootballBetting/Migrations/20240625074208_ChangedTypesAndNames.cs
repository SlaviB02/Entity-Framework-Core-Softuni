using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P02_FootballBetting.Migrations
{
    public partial class ChangedTypesAndNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStatistic_Games_GameId",
                table: "PlayerStatistic");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStatistic_Players_PlayerId",
                table: "PlayerStatistic");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerStatistic",
                table: "PlayerStatistic");

            migrationBuilder.RenameTable(
                name: "PlayerStatistic",
                newName: "PlayersStatistics");

            migrationBuilder.RenameColumn(
                name: "BetAmount",
                table: "Bets",
                newName: "Amount");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerStatistic_GameId",
                table: "PlayersStatistics",
                newName: "IX_PlayersStatistics_GameId");

            migrationBuilder.AlterColumn<int>(
                name: "Prediction",
                table: "Bets",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL (18,2)");

            migrationBuilder.AlterColumn<byte>(
                name: "ScoredGoals",
                table: "PlayersStatistics",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte>(
                name: "Assists",
                table: "PlayersStatistics",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayersStatistics",
                table: "PlayersStatistics",
                columns: new[] { "PlayerId", "GameId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlayersStatistics_Games_GameId",
                table: "PlayersStatistics",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayersStatistics_Players_PlayerId",
                table: "PlayersStatistics",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayersStatistics_Games_GameId",
                table: "PlayersStatistics");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayersStatistics_Players_PlayerId",
                table: "PlayersStatistics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayersStatistics",
                table: "PlayersStatistics");

            migrationBuilder.RenameTable(
                name: "PlayersStatistics",
                newName: "PlayerStatistic");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Bets",
                newName: "BetAmount");

            migrationBuilder.RenameIndex(
                name: "IX_PlayersStatistics_GameId",
                table: "PlayerStatistic",
                newName: "IX_PlayerStatistic_GameId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Prediction",
                table: "Bets",
                type: "DECIMAL (18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ScoredGoals",
                table: "PlayerStatistic",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<int>(
                name: "Assists",
                table: "PlayerStatistic",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerStatistic",
                table: "PlayerStatistic",
                columns: new[] { "PlayerId", "GameId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStatistic_Games_GameId",
                table: "PlayerStatistic",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStatistic_Players_PlayerId",
                table: "PlayerStatistic",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId");
        }
    }
}
