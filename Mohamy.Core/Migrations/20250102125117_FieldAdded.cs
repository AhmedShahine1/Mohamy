using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mohamy.Core.Migrations
{
    /// <inheritdoc />
    public partial class FieldAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                schema: "dbo",
                table: "Images",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_ApplicationUserId",
                schema: "dbo",
                table: "Images",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Users_ApplicationUserId",
                schema: "dbo",
                table: "Images",
                column: "ApplicationUserId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Users_ApplicationUserId",
                schema: "dbo",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_ApplicationUserId",
                schema: "dbo",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                schema: "dbo",
                table: "Images");
        }
    }
}
