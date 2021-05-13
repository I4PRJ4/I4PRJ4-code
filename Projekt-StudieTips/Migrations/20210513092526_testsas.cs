using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Projekt_StudieTips.Migrations
{
    public partial class testsas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tips",
                keyColumn: "TipId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tips",
                keyColumn: "TipId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tips",
                keyColumn: "TipId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tips",
                keyColumn: "TipId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tips",
                keyColumn: "TipId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Tips",
                keyColumn: "TipId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Tips",
                keyColumn: "TipId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Degrees",
                keyColumn: "DegreeId",
                keyValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Degrees",
                columns: new[] { "DegreeId", "DegreeName" },
                values: new object[] { 1, "Softwareteknologi" });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "CourseId", "CourseName", "DegreeId" },
                values: new object[,]
                {
                    { 1, "Generelt", 1 },
                    { 2, "Softwaretest", 1 },
                    { 3, "Database", 1 },
                    { 4, "GUI", 1 }
                });

            migrationBuilder.InsertData(
                table: "Tips",
                columns: new[] { "TipId", "CourseId", "Date", "Headline", "Text", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2021, 4, 22, 12, 50, 0, 0, DateTimeKind.Unspecified), "Gode idéer", "Nu skal i bare høre!", 1 },
                    { 2, 1, new DateTime(2021, 4, 22, 15, 12, 0, 0, DateTimeKind.Unspecified), "Billigste bøger?", "Læs overskrift?", 3 },
                    { 3, 1, new DateTime(2021, 4, 26, 5, 8, 0, 0, DateTimeKind.Unspecified), "Mcdonalds-snak", "Fish-O-Filet er det bedste", 2 },
                    { 4, 1, new DateTime(2021, 4, 18, 19, 36, 0, 0, DateTimeKind.Unspecified), "Tips til Zombs", "jeg er dårlig, søger et team, træning hver onsdag kl. 20", 4 },
                    { 5, 2, new DateTime(2021, 4, 23, 8, 3, 0, 0, DateTimeKind.Unspecified), "Ladeskabsopgaven", "Forstår den ikk", 2 },
                    { 6, 3, new DateTime(2021, 4, 24, 20, 28, 0, 0, DateTimeKind.Unspecified), "Hjælp til DAB", "Jeg har virkelig brug for hjælp til DAB Assignment 2", 1 },
                    { 7, 3, new DateTime(2021, 4, 27, 10, 38, 0, 0, DateTimeKind.Unspecified), "Assignment 3", "hvad sker der", 3 }
                });
        }
    }
}
