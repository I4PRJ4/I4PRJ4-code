using Microsoft.EntityFrameworkCore.Migrations;

namespace Projekt_StudieTips.Migrations
{
    public partial class ReadyForModerationPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Tips",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Tips");
        }
    }
}
