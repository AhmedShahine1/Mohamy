using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mohamy.Core.Migrations
{
    /// <inheritdoc />
    public partial class IgnoredTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IgnoredConsultations",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LawyerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    consultingId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IgnoredConsultations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IgnoredConsultations_Consultings_consultingId",
                        column: x => x.consultingId,
                        principalSchema: "dbo",
                        principalTable: "Consultings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IgnoredConsultations_Users_LawyerId",
                        column: x => x.LawyerId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IgnoredConsultations_consultingId",
                schema: "dbo",
                table: "IgnoredConsultations",
                column: "consultingId");

            migrationBuilder.CreateIndex(
                name: "IX_IgnoredConsultations_LawyerId",
                schema: "dbo",
                table: "IgnoredConsultations",
                column: "LawyerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IgnoredConsultations",
                schema: "dbo");
        }
    }
}
