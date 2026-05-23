using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRLConstructionManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSupervisorCredits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupervisorCredits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupervisorName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupervisorCredits", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorCredits_Date",
                table: "SupervisorCredits",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorCredits_PaymentMode",
                table: "SupervisorCredits",
                column: "PaymentMode");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorCredits_SupervisorName",
                table: "SupervisorCredits",
                column: "SupervisorName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupervisorCredits");
        }
    }
}
