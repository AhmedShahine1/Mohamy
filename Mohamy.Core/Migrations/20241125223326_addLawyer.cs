using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mohamy.Core.Migrations
{
    /// <inheritdoc />
    public partial class addLawyer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Images_ProfileId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BeneficiaryName",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IBAN",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lawyerLicenseId",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "yearsExperience",
                schema: "dbo",
                table: "Users",
                type: "int",
                nullable: true);

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
                    LawyerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LawyerLicenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LawyerLicenses_Users_LawyerId",
                        column: x => x.LawyerId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MainConsultings",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    iconId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainConsultings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MainConsultings_Images_iconId",
                        column: x => x.iconId,
                        principalSchema: "dbo",
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubConsultings",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    iconId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MainConsultingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubConsultings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubConsultings_Images_iconId",
                        column: x => x.iconId,
                        principalSchema: "dbo",
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubConsultings_MainConsultings_MainConsultingId",
                        column: x => x.MainConsultingId,
                        principalSchema: "dbo",
                        principalTable: "MainConsultings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Experience",
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
                    table.PrimaryKey("PK_Experience", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Experience_SubConsultings_subConsultingId",
                        column: x => x.subConsultingId,
                        principalSchema: "dbo",
                        principalTable: "SubConsultings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Experience_Users_LawyerId",
                        column: x => x.LawyerId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Specialties",
                schema: "dbo",
                columns: table => new
                {
                    LawyerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    subConsultingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => new { x.LawyerId, x.subConsultingId });
                    table.ForeignKey(
                        name: "FK_Specialties_SubConsultings_subConsultingId",
                        column: x => x.subConsultingId,
                        principalSchema: "dbo",
                        principalTable: "SubConsultings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Specialties_Users_LawyerId",
                        column: x => x.LawyerId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Experience_LawyerId",
                schema: "dbo",
                table: "Experience",
                column: "LawyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Experience_subConsultingId",
                schema: "dbo",
                table: "Experience",
                column: "subConsultingId");

            migrationBuilder.CreateIndex(
                name: "IX_graduationCertificate_LawyerId",
                schema: "dbo",
                table: "graduationCertificate",
                column: "LawyerId");

            migrationBuilder.CreateIndex(
                name: "IX_LawyerLicenses_LawyerId",
                schema: "dbo",
                table: "LawyerLicenses",
                column: "LawyerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MainConsultings_iconId",
                schema: "dbo",
                table: "MainConsultings",
                column: "iconId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_subConsultingId",
                schema: "dbo",
                table: "Specialties",
                column: "subConsultingId");

            migrationBuilder.CreateIndex(
                name: "IX_SubConsultings_iconId",
                schema: "dbo",
                table: "SubConsultings",
                column: "iconId");

            migrationBuilder.CreateIndex(
                name: "IX_SubConsultings_MainConsultingId",
                schema: "dbo",
                table: "SubConsultings",
                column: "MainConsultingId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Images_ProfileId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Experience",
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

            migrationBuilder.DropTable(
                name: "SubConsultings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MainConsultings",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BankName",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BeneficiaryName",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "City",
                schema: "dbo",
                table: "Users");

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
                name: "Description",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IBAN",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Region",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "lawyerLicenseId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "yearsExperience",
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
