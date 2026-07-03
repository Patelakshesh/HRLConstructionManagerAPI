using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HRLConstructionManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedOn", "Enable", "ModifiedOn", "RoleName" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), true, null, "admin" },
                    { 2, new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), true, null, "supervision" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "CreatedBy", "CreatedOn", "Email", "Enable", "MobileNumber", "ModifiedBy", "ModifiedOn", "Name", "Password", "RoleId" },
                values: new object[,]
                {
                    { 1, "Head Office", "system", new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), "admin@example.com", true, "9999999999", null, null, "Admin User", "admin", 1 },
                    { 2, "Site Office", "system", new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), "supervision@example.com", true, "8888888888", null, null, "Supervision User", "supervision", 2 }
                });
        }
    }
}
