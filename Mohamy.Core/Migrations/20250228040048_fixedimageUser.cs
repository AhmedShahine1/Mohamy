using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mohamy.Core.Migrations
{
    /// <inheritdoc />
    public partial class fixedimageUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_ProfileId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfileId",
                schema: "dbo",
                table: "Users",
                column: "ProfileId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_ProfileId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfileId",
                schema: "dbo",
                table: "Users",
                column: "ProfileId");
        }
    }
}
