using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineTesting.Migrations
{
    public partial class AddingFieldInTestEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ForCity",
                table: "Test",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ForCountry",
                table: "Test",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Test",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Photo",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForCity",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "ForCountry",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Test");

            migrationBuilder.AlterColumn<string>(
                name: "Photo",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);
        }
    }
}
