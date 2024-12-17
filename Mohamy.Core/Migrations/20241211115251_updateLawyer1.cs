using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mohamy.Core.Migrations
{
    /// <inheritdoc />
    public partial class updateLawyer1 : Migration
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
                name: "FK_Specialties_SubConsultings_subConsultingId",
                schema: "dbo",
                table: "Specialties");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialties_Users_LawyerId",
                schema: "dbo",
                table: "Specialties");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Images_ProfileId",
                schema: "dbo",
                table: "Users");

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
                name: "FK_Users_Images_ProfileId",
                schema: "dbo",
                table: "Users");

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
                name: "FK_Users_Images_ProfileId",
                schema: "dbo",
                table: "Users",
                column: "ProfileId",
                principalSchema: "dbo",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
