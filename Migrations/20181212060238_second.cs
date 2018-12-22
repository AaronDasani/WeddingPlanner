using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeddingPlanner.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "weddingDate",
                table: "Weddings",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "weddingAdress",
                table: "Weddings",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "wedder_two",
                table: "Weddings",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "wedder_one",
                table: "Weddings",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "weddingDate",
                table: "Weddings",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "weddingAdress",
                table: "Weddings",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "wedder_two",
                table: "Weddings",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "wedder_one",
                table: "Weddings",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
