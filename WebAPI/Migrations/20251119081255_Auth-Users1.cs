using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AuthUsers1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Rentals",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            //migrationBuilder.AddColumn<int>(
            //    name: "UserId",
            //    table: "Rentals",
            //    type: "integer",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.CreateTable(
            //    name: "Users",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //        Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //        PasswordHash = table.Column<string>(type: "text", nullable: false),
            //        Role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
            //        CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Users", x => x.Id);
            //    });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] {"CreatedAt", "Email", "PasswordHash", "Role", "UpdatedAt", "Username" },
                values: new object[] {new DateTime(2025, 11, 19, 8, 12, 55, 97, DateTimeKind.Utc).AddTicks(5991), "admin@carrental.com", "AQAAAAIAAYagAAAAELEVZv9sDCIgU5uJF4P6bq9dHAHi739l/0OymGGwX1QufaIerskwK0qOAUnmSvAX8w==", "Admin", null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_UserId",
                table: "Rentals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Rentals_Users_UserId",
            //    table: "Rentals",
            //    column: "UserId",
            //    principalTable: "Users",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Users_UserId",
                table: "Rentals");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_UserId",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Rentals");
        }
    }
}
