using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mohamy.Core.Migrations
{
    /// <inheritdoc />
    public partial class ReviewsFieldAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConsultingId",
                schema: "dbo",
                table: "Evaluations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_ConsultingId",
                schema: "dbo",
                table: "Evaluations",
                column: "ConsultingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluations_Consultings_ConsultingId",
                schema: "dbo",
                table: "Evaluations",
                column: "ConsultingId",
                principalSchema: "dbo",
                principalTable: "Consultings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluations_Consultings_ConsultingId",
                schema: "dbo",
                table: "Evaluations");

            migrationBuilder.DropIndex(
                name: "IX_Evaluations_ConsultingId",
                schema: "dbo",
                table: "Evaluations");

            migrationBuilder.DropColumn(
                name: "ConsultingId",
                schema: "dbo",
                table: "Evaluations");
        }
    }
}
