using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineTesting.Migrations
{
    public partial class updateStudentEntity2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IP",
                table: "Student",
                newName: "NetworkIP");

            migrationBuilder.AddColumn<string>(
                name: "LocalIP",
                table: "Student",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalIP",
                table: "Student");

            migrationBuilder.RenameColumn(
                name: "NetworkIP",
                table: "Student",
                newName: "IP");
        }
    }
}
