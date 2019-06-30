using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineTesting.Migrations
{
    public partial class updateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForCity",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "ForCountry",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Student");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Student",
                newName: "StudentToTestId");

            migrationBuilder.AlterColumn<string>(
                name: "StudentToTestId",
                table: "Student",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "StudentToTests",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    TestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentToTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentToTests_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentToTests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Student_StudentToTestId",
                table: "Student",
                column: "StudentToTestId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentToTests_TestId",
                table: "StudentToTests",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentToTests_UserId",
                table: "StudentToTests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_StudentToTests_StudentToTestId",
                table: "Student",
                column: "StudentToTestId",
                principalTable: "StudentToTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_StudentToTests_StudentToTestId",
                table: "Student");

            migrationBuilder.DropTable(
                name: "StudentToTests");

            migrationBuilder.DropIndex(
                name: "IX_Student_StudentToTestId",
                table: "Student");

            migrationBuilder.RenameColumn(
                name: "StudentToTestId",
                table: "Student",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "ForCity",
                table: "Test",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ForCountry",
                table: "Test",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Student",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Student",
                nullable: true);
        }
    }
}
