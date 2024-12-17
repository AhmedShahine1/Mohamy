using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mohamy.Core.Migrations
{
    /// <inheritdoc />
    public partial class updateUserAndConsulting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostConsulting30",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CostConsulting60",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CostConsulting90",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "timeConsulting",
                schema: "dbo",
                table: "Consultings");

            migrationBuilder.AddColumn<bool>(
                name: "voiceConsulting",
                schema: "dbo",
                table: "Consultings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "voiceConsulting",
                schema: "dbo",
                table: "Consultings");

            migrationBuilder.AddColumn<decimal>(
                name: "CostConsulting30",
                schema: "dbo",
                table: "Users",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CostConsulting60",
                schema: "dbo",
                table: "Users",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CostConsulting90",
                schema: "dbo",
                table: "Users",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "timeConsulting",
                schema: "dbo",
                table: "Consultings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
