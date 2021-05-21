using Microsoft.EntityFrameworkCore.Migrations;

namespace Projekt_StudieTips.Migrations
{
    public partial class ChangedSomething : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Courses_DegreeId",
                table: "Courses",
                column: "DegreeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Degrees_DegreeId",
                table: "Courses",
                column: "DegreeId",
                principalTable: "Degrees",
                principalColumn: "DegreeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Degrees_DegreeId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_DegreeId",
                table: "Courses");
        }
    }
}
