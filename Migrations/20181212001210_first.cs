using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeddingPlanner.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    user_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    firstname = table.Column<string>(nullable: false),
                    lastname = table.Column<string>(nullable: false),
                    email = table.Column<string>(nullable: false),
                    password = table.Column<string>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "Weddings",
                columns: table => new
                {
                    wedding_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    wedder_one = table.Column<string>(nullable: true),
                    wedder_two = table.Column<string>(nullable: true),
                    weddingDate = table.Column<DateTime>(nullable: false),
                    weddingAdress = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    user_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weddings", x => x.wedding_id);
                    table.ForeignKey(
                        name: "FK_Weddings_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeddingGuests",
                columns: table => new
                {
                    association_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(nullable: false),
                    wedding_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeddingGuests", x => x.association_id);
                    table.ForeignKey(
                        name: "FK_WeddingGuests_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeddingGuests_Weddings_wedding_id",
                        column: x => x.wedding_id,
                        principalTable: "Weddings",
                        principalColumn: "wedding_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeddingGuests_user_id",
                table: "WeddingGuests",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_WeddingGuests_wedding_id",
                table: "WeddingGuests",
                column: "wedding_id");

            migrationBuilder.CreateIndex(
                name: "IX_Weddings_user_id",
                table: "Weddings",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeddingGuests");

            migrationBuilder.DropTable(
                name: "Weddings");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
