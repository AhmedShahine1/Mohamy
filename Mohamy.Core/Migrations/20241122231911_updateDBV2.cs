using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mohamy.Core.Migrations
{
    /// <inheritdoc />
    public partial class updateDBV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Consultings_ConsultingId",
                schema: "dbo",
                table: "Images");

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

            migrationBuilder.DropTable(
                name: "Experiences",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RequestConsultings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Consultings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SubConsultings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MainConsultings",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Users_graduationCertificateId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_lawyerLicenseId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Images_ConsultingId",
                schema: "dbo",
                table: "Images");

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
                name: "CostConsulting",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Education",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IBAN",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "academicSpecialization",
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

            migrationBuilder.DropColumn(
                name: "yearsExperience",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ConsultingId",
                schema: "dbo",
                table: "Images");

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
                name: "CostConsulting",
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
                name: "Education",
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
                name: "academicSpecialization",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.AddColumn<int>(
                name: "yearsExperience",
                schema: "dbo",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConsultingId",
                schema: "dbo",
                table: "Images",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MainConsultings",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    iconId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubConsultings",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    iconId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MainConsultingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubConsultings_MainConsultings_MainConsultingId",
                        column: x => x.MainConsultingId,
                        principalSchema: "dbo",
                        principalTable: "MainConsultings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Consultings",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LawyerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    subConsultingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    PriceService = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    statusConsulting = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consultings_SubConsultings_subConsultingId",
                        column: x => x.subConsultingId,
                        principalSchema: "dbo",
                        principalTable: "SubConsultings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Consultings_Users_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Consultings_Users_LawyerId",
                        column: x => x.LawyerId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Experiences",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LawyerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    subConsultingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Experiences_Users_LawyerId",
                        column: x => x.LawyerId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestConsultings",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConsultingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LawyerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    statusRequestConsulting = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestConsultings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestConsultings_Consultings_ConsultingId",
                        column: x => x.ConsultingId,
                        principalSchema: "dbo",
                        principalTable: "Consultings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestConsultings_Users_LawyerId",
                        column: x => x.LawyerId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Images_ConsultingId",
                schema: "dbo",
                table: "Images",
                column: "ConsultingId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultings_CustomerId",
                schema: "dbo",
                table: "Consultings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultings_LawyerId",
                schema: "dbo",
                table: "Consultings",
                column: "LawyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultings_subConsultingId",
                schema: "dbo",
                table: "Consultings",
                column: "subConsultingId");

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
                name: "IX_MainConsultings_iconId",
                schema: "dbo",
                table: "MainConsultings",
                column: "iconId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestConsultings_ConsultingId",
                schema: "dbo",
                table: "RequestConsultings",
                column: "ConsultingId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestConsultings_LawyerId",
                schema: "dbo",
                table: "RequestConsultings",
                column: "LawyerId");

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
                name: "FK_Images_Consultings_ConsultingId",
                schema: "dbo",
                table: "Images",
                column: "ConsultingId",
                principalSchema: "dbo",
                principalTable: "Consultings",
                principalColumn: "Id");

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
    }
}
