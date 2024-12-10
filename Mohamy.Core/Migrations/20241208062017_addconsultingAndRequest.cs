using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mohamy.Core.Migrations
{
    /// <inheritdoc />
    public partial class addconsultingAndRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConsultingId",
                schema: "dbo",
                table: "Images",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Consultings",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    subConsultingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LawyerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceService = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    statusConsulting = table.Column<int>(type: "int", nullable: false),
                    timeConsulting = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Consultings_Users_LawyerId",
                        column: x => x.LawyerId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RequestConsultings",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LawyerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConsultingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PriceService = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    statusRequestConsulting = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestConsultings_Users_LawyerId",
                        column: x => x.LawyerId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "IX_RequestConsultings_ConsultingId",
                schema: "dbo",
                table: "RequestConsultings",
                column: "ConsultingId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestConsultings_LawyerId",
                schema: "dbo",
                table: "RequestConsultings",
                column: "LawyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Consultings_ConsultingId",
                schema: "dbo",
                table: "Images",
                column: "ConsultingId",
                principalSchema: "dbo",
                principalTable: "Consultings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Consultings_ConsultingId",
                schema: "dbo",
                table: "Images");

            migrationBuilder.DropTable(
                name: "RequestConsultings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Consultings",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Images_ConsultingId",
                schema: "dbo",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ConsultingId",
                schema: "dbo",
                table: "Images");
        }
    }
}
