using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class removeredundantentityrules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SavedGames_SavedGameConfigurations_ConfigurationId",
                table: "SavedGames");

            migrationBuilder.AddForeignKey(
                name: "FK_SavedGames_SavedGameConfigurations_ConfigurationId",
                table: "SavedGames",
                column: "ConfigurationId",
                principalTable: "SavedGameConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SavedGames_SavedGameConfigurations_ConfigurationId",
                table: "SavedGames");

            migrationBuilder.AddForeignKey(
                name: "FK_SavedGames_SavedGameConfigurations_ConfigurationId",
                table: "SavedGames",
                column: "ConfigurationId",
                principalTable: "SavedGameConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
