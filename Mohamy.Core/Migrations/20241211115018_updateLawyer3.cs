using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mohamy.Core.Migrations
{
    /// <inheritdoc />
    public partial class updateLawyer3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "lawyerLicenseId",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Experiences",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    subConsultingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LawyerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Experiences_SubConsultings_subConsultingId",
                        column: x => x.subConsultingId,
                        principalSchema: "dbo",
                        principalTable: "SubConsultings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Experiences_Users_LawyerId",
                        column: x => x.LawyerId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "graduationCertificate",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Collage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    University = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LawyerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_graduationCertificate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_graduationCertificate_Users_LawyerId",
                        column: x => x.LawyerId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LawyerLicenses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LawyerId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LawyerLicenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialties",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LawyerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    subConsultingId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Specialties_SubConsultings_subConsultingId",
                        column: x => x.subConsultingId,
                        principalSchema: "dbo",
                        principalTable: "SubConsultings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Specialties_Users_LawyerId",
                        column: x => x.LawyerId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_lawyerLicenseId",
                schema: "dbo",
                table: "Users",
                column: "lawyerLicenseId",
                unique: true,
                filter: "[lawyerLicenseId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Experiences_LawyerId",
                schema: "dbo",
                table: "Experiences",
                column: "LawyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Experiences_subConsultingId",
                schema: "dbo",
                table: "Experiences",
                column: "subConsultingId");

            migrationBuilder.CreateIndex(
                name: "IX_graduationCertificate_LawyerId",
                schema: "dbo",
                table: "graduationCertificate",
                column: "LawyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_LawyerId",
                schema: "dbo",
                table: "Specialties",
                column: "LawyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_subConsultingId",
                schema: "dbo",
                table: "Specialties",
                column: "subConsultingId");

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
                name: "FK_Users_LawyerLicenses_lawyerLicenseId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Experiences",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "graduationCertificate",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "LawyerLicenses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Specialties",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Users_lawyerLicenseId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "lawyerLicenseId",
                schema: "dbo",
                table: "Users");
        }
    }
}
