using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeddingPlanner.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "weddingDate",
                table: "Weddings",
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "weddingDate",
                table: "Weddings",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
