using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mohamy.Core.Migrations
{
    /// <inheritdoc />
    public partial class editapplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Images_ProfileId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "graduationCertificateId",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lawyerLicenseId",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_graduationCertificateId",
                schema: "dbo",
                table: "Users",
                column: "graduationCertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_lawyerLicenseId",
                schema: "dbo",
                table: "Users",
                column: "lawyerLicenseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Images_ProfileId",
                schema: "dbo",
                table: "Users",
                column: "ProfileId",
                principalSchema: "dbo",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Images_graduationCertificateId",
                schema: "dbo",
                table: "Users",
                column: "graduationCertificateId",
                principalSchema: "dbo",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Images_lawyerLicenseId",
                schema: "dbo",
                table: "Users",
                column: "lawyerLicenseId",
                principalSchema: "dbo",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Images_ProfileId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Images_graduationCertificateId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Images_lawyerLicenseId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_graduationCertificateId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_lawyerLicenseId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "graduationCertificateId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "lawyerLicenseId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Images_ProfileId",
                schema: "dbo",
                table: "Users",
                column: "ProfileId",
                principalSchema: "dbo",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
