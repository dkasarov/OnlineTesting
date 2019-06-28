using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineTesting.Migrations
{
    public partial class updateStudentEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Lat",
                table: "Student",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Lng",
                table: "Student",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Student",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lat",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "Lng",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Student");
        }
    }
}
