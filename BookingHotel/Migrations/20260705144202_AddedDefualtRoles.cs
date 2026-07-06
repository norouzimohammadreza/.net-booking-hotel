using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingHotel.Migrations
{
    /// <inheritdoc />
    public partial class AddedDefualtRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "012480c1-2e64-43a4-a30c-f931d1c0920a", "012480c1-2e64-43a4-a30c-f931d1c0920a", "Admin", "ADMIN" },
                    { "e3d430c7-6604-44a5-94b1-07bcf5d0fbc8", "e3d430c7-6604-44a5-94b1-07bcf5d0fbc8", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "012480c1-2e64-43a4-a30c-f931d1c0920a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e3d430c7-6604-44a5-94b1-07bcf5d0fbc8");
        }
    }
}
