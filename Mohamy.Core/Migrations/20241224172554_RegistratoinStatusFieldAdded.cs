using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mohamy.Core.Migrations
{
    /// <inheritdoc />
    public partial class RegistratoinStatusFieldAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegistrationStatus",
                schema: "dbo",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationStatus",
                schema: "dbo",
                table: "Users");
        }
    }
}
