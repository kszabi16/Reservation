using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Reservation.Migrations
{
    /// <inheritdoc />
    public partial class DataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 15);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Amenities",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Wifi" },
                    { 2, "Klíma" },
                    { 3, "Ingyenes parkolás" },
                    { 4, "Szauna" },
                    { 5, "Állatbarát" },
                    { 6, "Medence" },
                    { 7, "Pezsgőfürdő (Jakuzzi)" },
                    { 8, "Erkély / Terasz" },
                    { 9, "Felszerelt konyha" },
                    { 10, "Mosógép" },
                    { 11, "Okostévé (Netflix, stb.)" },
                    { 12, "Kert" },
                    { 13, "Grillezési lehetőség" },
                    { 14, "Reggeli biztosított" },
                    { 15, "Elektromos autó töltő" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AvatarUrl", "Bio", "CreatedAt", "Deleted", "Email", "IsTrustedHost", "Location", "PasswordHash", "PhoneNumber", "Role", "Username" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2024, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), false, "bela@example.com", false, null, "hashed_pw", null, 1, "Kovács Béla" },
                    { 2, null, null, new DateTime(2024, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), false, "anna@example.com", false, null, "hashed_pw", null, 1, "Tóth Anna" }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "Capacity", "CreatedAt", "Deleted", "Description", "HostId", "ImageUrl", "IsApproved", "Location", "PricePerNight", "Title" },
                values: new object[,]
                {
                    { 1, 3, new DateTime(2024, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), false, "Modern apartman a Deák tér közelében, remek kilátással.", 2, null, true, "Budapest", 25000m, "Budapest belvárosi apartman" },
                    { 2, 6, new DateTime(2024, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), false, "Kényelmes ház közvetlenül a Balaton partján.", 2, null, true, "Siófok", 45000m, "Balatoni nyaraló" },
                    { 3, 5, new DateTime(2024, 10, 14, 0, 0, 0, 0, DateTimeKind.Utc), false, "Hangulatos faház kandallóval és panorámás terasszal.", 2, null, true, "Mátraháza", 32000m, "Hegyi faház Mátrában" }
                });
        }
    }
}
