using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mohamy.Core.Migrations
{
    /// <inheritdoc />
    public partial class OnlineFieldAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Online",
                schema: "dbo",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Online",
                schema: "dbo",
                table: "Users");
        }
    }
}
