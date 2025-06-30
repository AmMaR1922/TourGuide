using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class EditRefToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpireOne",
                table: "RefreshToken",
                newName: "ExpireOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpireOn",
                table: "RefreshToken",
                newName: "ExpireOne");
        }
    }
}
