using Microsoft.EntityFrameworkCore.Migrations;

namespace Projekt_StudieTips.Migrations
{
    public partial class RemovedManualUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tips_Users_UserId",
                table: "Tips");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Tips_UserId",
                table: "Tips");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tips");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Tips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Tips");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Tips",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Username" },
                values: new object[,]
                {
                    { 1, "TronaldDump" },
                    { 2, "SwedishNoob" },
                    { 3, "TheLegend" },
                    { 4, "IntrovertedSnail" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tips_UserId",
                table: "Tips",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tips_Users_UserId",
                table: "Tips",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
