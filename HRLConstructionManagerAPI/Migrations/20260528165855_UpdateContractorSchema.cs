using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRLConstructionManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContractorSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactPerson",
                table: "Contractors");

            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "Contractors",
                newName: "ContractorName");

            migrationBuilder.RenameIndex(
                name: "IX_Contractors_CompanyName",
                table: "Contractors",
                newName: "IX_Contractors_ContractorName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContractorName",
                table: "Contractors",
                newName: "CompanyName");

            migrationBuilder.RenameIndex(
                name: "IX_Contractors_ContractorName",
                table: "Contractors",
                newName: "IX_Contractors_CompanyName");

            migrationBuilder.AddColumn<string>(
                name: "ContactPerson",
                table: "Contractors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
