using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRLConstructionManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddReceiptImageToSupervisorCredit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiptImage",
                table: "SupervisorCredits",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiptImage",
                table: "SupervisorCredits");
        }
    }
}
