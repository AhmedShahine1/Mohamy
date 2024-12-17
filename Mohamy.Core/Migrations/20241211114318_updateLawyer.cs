using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mohamy.Core.Migrations
{
    /// <inheritdoc />
    public partial class updateLawyer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_SubConsultings_subConsultingId",
                schema: "dbo",
                table: "Experiences");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_Users_LawyerId",
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
                name: "FK_Specialties_Users_LawyerId",
                schema: "dbo",
                table: "Specialties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Specialties",
                schema: "dbo",
                table: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_LawyerLicenses_LawyerId",
                schema: "dbo",
                table: "LawyerLicenses");

            migrationBuilder.AlterColumn<string>(
                name: "lawyerLicenseId",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "dbo",
                table: "Specialties",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LawyerId",
                schema: "dbo",
                table: "LawyerLicenses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Specialties",
                schema: "dbo",
                table: "Specialties",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_lawyerLicenseId",
                schema: "dbo",
                table: "Users",
                column: "lawyerLicenseId",
                unique: true,
                filter: "[lawyerLicenseId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_LawyerId",
                schema: "dbo",
                table: "Specialties",
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
                name: "FK_Experiences_Users_LawyerId",
                schema: "dbo",
                table: "Experiences",
                column: "LawyerId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_Specialties_Users_LawyerId",
                schema: "dbo",
                table: "Specialties",
                column: "LawyerId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_SubConsultings_subConsultingId",
                schema: "dbo",
                table: "Experiences");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_Users_LawyerId",
                schema: "dbo",
                table: "Experiences");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialties_SubConsultings_subConsultingId",
                schema: "dbo",
                table: "Specialties");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialties_Users_LawyerId",
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_Specialties",
                schema: "dbo",
                table: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_Specialties_LawyerId",
                schema: "dbo",
                table: "Specialties");

            migrationBuilder.AlterColumn<string>(
                name: "lawyerLicenseId",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "dbo",
                table: "Specialties",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LawyerId",
                schema: "dbo",
                table: "LawyerLicenses",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Specialties",
                schema: "dbo",
                table: "Specialties",
                columns: new[] { "LawyerId", "subConsultingId" });

            migrationBuilder.CreateIndex(
                name: "IX_LawyerLicenses_LawyerId",
                schema: "dbo",
                table: "LawyerLicenses",
                column: "LawyerId",
                unique: true);

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
                name: "FK_Experiences_Users_LawyerId",
                schema: "dbo",
                table: "Experiences",
                column: "LawyerId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Specialties_Users_LawyerId",
                schema: "dbo",
                table: "Specialties",
                column: "LawyerId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
