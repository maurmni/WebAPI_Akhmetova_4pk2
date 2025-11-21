using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Users_Added3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReturnDate",
                table: "Rentals",
                newName: "EndDate");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Rentals",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 20, 7, 18, 40, 296, DateTimeKind.Utc).AddTicks(7188), "AQAAAAIAAYagAAAAEL1vq4SU8yajkbFTLDnS8s6zheC/ZqT3/fiVywb2SYeaotHK9UCVkOYoAspMMALdpQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Rentals",
                newName: "ReturnDate");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Rentals",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 20, 6, 12, 37, 831, DateTimeKind.Utc).AddTicks(2257), "AQAAAAIAAYagAAAAEHUJrQFcWqObwD2Z7z222Cs6vtKpNUW8xcha9NqNgCy4MavBg7GRIfxC1ACwVOCxnQ==" });
        }
    }
}
