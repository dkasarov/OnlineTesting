using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineTesting.Migrations
{
    public partial class updateTestQuestionTypeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "TestQuestionType",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "TestQuestionType");
        }
    }
}
