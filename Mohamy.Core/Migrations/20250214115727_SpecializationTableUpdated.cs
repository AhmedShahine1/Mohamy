using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mohamy.Core.Migrations
{
    /// <inheritdoc />
    public partial class SpecializationTableUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Specialties_MainConsultings_mainConsultingId",
                schema: "dbo",
                table: "Specialties");

            migrationBuilder.RenameColumn(
                name: "mainConsultingId",
                schema: "dbo",
                table: "Specialties",
                newName: "subConsultingId");

            migrationBuilder.RenameIndex(
                name: "IX_Specialties_mainConsultingId",
                schema: "dbo",
                table: "Specialties",
                newName: "IX_Specialties_subConsultingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Specialties_SubConsultings_subConsultingId",
                schema: "dbo",
                table: "Specialties",
                column: "subConsultingId",
                principalSchema: "dbo",
                principalTable: "SubConsultings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Specialties_SubConsultings_subConsultingId",
                schema: "dbo",
                table: "Specialties");

            migrationBuilder.RenameColumn(
                name: "subConsultingId",
                schema: "dbo",
                table: "Specialties",
                newName: "mainConsultingId");

            migrationBuilder.RenameIndex(
                name: "IX_Specialties_subConsultingId",
                schema: "dbo",
                table: "Specialties",
                newName: "IX_Specialties_mainConsultingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Specialties_MainConsultings_mainConsultingId",
                schema: "dbo",
                table: "Specialties",
                column: "mainConsultingId",
                principalSchema: "dbo",
                principalTable: "MainConsultings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
