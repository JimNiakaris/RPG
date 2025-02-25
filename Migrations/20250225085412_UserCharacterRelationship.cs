using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNet_RPG.Migrations
{
    /// <inheritdoc />
    public partial class UserCharacterRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Character",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Character_UserId",
                table: "Character",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Character_Users_UserId",
                table: "Character",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Character_Users_UserId",
                table: "Character");

            migrationBuilder.DropIndex(
                name: "IX_Character_UserId",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Character");
        }
    }
}
