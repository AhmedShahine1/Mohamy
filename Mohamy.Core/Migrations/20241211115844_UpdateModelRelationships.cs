using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mohamy.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_SubConsultings_subConsultingId",
                schema: "dbo",
                table: "Experiences");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialties_SubConsultings_subConsultingId",
                schema: "dbo",
                table: "Specialties");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_LawyerLicenses_lawyerLicenseId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_lawyerLicenseId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "LawyerId",
                schema: "dbo",
                table: "LawyerLicenses",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Users_lawyerLicenseId",
                schema: "dbo",
                table: "Users",
                column: "lawyerLicenseId");

            migrationBuilder.CreateIndex(
                name: "IX_LawyerLicenses_LawyerId",
                schema: "dbo",
                table: "LawyerLicenses",
                column: "LawyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiences_SubConsultings_subConsultingId",
                schema: "dbo",
                table: "Experiences",
                column: "subConsultingId",
                principalSchema: "dbo",
                principalTable: "SubConsultings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LawyerLicenses_Users_LawyerId",
                schema: "dbo",
                table: "LawyerLicenses",
                column: "LawyerId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Specialties_SubConsultings_subConsultingId",
                schema: "dbo",
                table: "Specialties",
                column: "subConsultingId",
                principalSchema: "dbo",
                principalTable: "SubConsultings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_LawyerLicenses_lawyerLicenseId",
                schema: "dbo",
                table: "Users",
                column: "lawyerLicenseId",
                principalSchema: "dbo",
                principalTable: "LawyerLicenses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_SubConsultings_subConsultingId",
                schema: "dbo",
                table: "Experiences");

            migrationBuilder.DropForeignKey(
                name: "FK_LawyerLicenses_Users_LawyerId",
                schema: "dbo",
                table: "LawyerLicenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialties_SubConsultings_subConsultingId",
                schema: "dbo",
                table: "Specialties");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_LawyerLicenses_lawyerLicenseId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_lawyerLicenseId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_LawyerLicenses_LawyerId",
                schema: "dbo",
                table: "LawyerLicenses");

            migrationBuilder.AlterColumn<string>(
                name: "LawyerId",
                schema: "dbo",
                table: "LawyerLicenses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Users_lawyerLicenseId",
                schema: "dbo",
                table: "Users",
                column: "lawyerLicenseId",
                unique: true,
                filter: "[lawyerLicenseId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiences_SubConsultings_subConsultingId",
                schema: "dbo",
                table: "Experiences",
                column: "subConsultingId",
                principalSchema: "dbo",
                principalTable: "SubConsultings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Specialties_SubConsultings_subConsultingId",
                schema: "dbo",
                table: "Specialties",
                column: "subConsultingId",
                principalSchema: "dbo",
                principalTable: "SubConsultings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_LawyerLicenses_lawyerLicenseId",
                schema: "dbo",
                table: "Users",
                column: "lawyerLicenseId",
                principalSchema: "dbo",
                principalTable: "LawyerLicenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
