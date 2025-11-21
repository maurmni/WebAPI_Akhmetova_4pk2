using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Users_Added2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DriverLicense",
                table: "Renters",
                newName: "DriverLicenseNumber");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualReturnDate",
                table: "Rentals",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Email", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 20, 6, 12, 37, 831, DateTimeKind.Utc).AddTicks(2257), "admin@gmail.com", "AQAAAAIAAYagAAAAEHUJrQFcWqObwD2Z7z222Cs6vtKpNUW8xcha9NqNgCy4MavBg7GRIfxC1ACwVOCxnQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualReturnDate",
                table: "Rentals");

            migrationBuilder.RenameColumn(
                name: "DriverLicenseNumber",
                table: "Renters",
                newName: "DriverLicense");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Email", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 19, 8, 12, 55, 97, DateTimeKind.Utc).AddTicks(5991), "admin@carrental.com", "AQAAAAIAAYagAAAAELEVZv9sDCIgU5uJF4P6bq9dHAHi739l/0OymGGwX1QufaIerskwK0qOAUnmSvAX8w==" });
        }
    }
}
