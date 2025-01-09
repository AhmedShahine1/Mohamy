using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mohamy.Core.Migrations
{
    /// <inheritdoc />
    public partial class DescriptionFieldAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "RequestConsultings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "dbo",
                table: "RequestConsultings");
        }
    }
}
