using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRLConstructionManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAttendanceImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Attendances",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Attendances");
        }
    }
}
