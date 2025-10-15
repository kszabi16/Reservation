using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Reservation.Migrations
{
    /// <inheritdoc />
    public partial class Példa_adatok : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Deleted", "Email", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, false, "bela@example.com", "hashed_pw", 1, "Kovács Béla" },
                    { 2, false, "anna@example.com", "hashed_pw", 1, "Tóth Anna" }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "Capacity", "CreatedAt", "Deleted", "Description", "HostId", "Location", "PricePerNight", "Title" },
                values: new object[,]
                {
                    { 1, 3, new DateTime(2024, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Modern apartman a Deák tér közelében, remek kilátással.", 2, "Budapest", 25000m, "Budapest belvárosi apartman" },
                    { 2, 6, new DateTime(2024, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Kényelmes ház közvetlenül a Balaton partján.", 2, "Siófok", 45000m, "Balatoni nyaraló" },
                    { 3, 5, new DateTime(2024, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Hangulatos faház kandallóval és panorámás terasszal.", 2, "Mátraháza", 32000m, "Hegyi faház Mátrában" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
