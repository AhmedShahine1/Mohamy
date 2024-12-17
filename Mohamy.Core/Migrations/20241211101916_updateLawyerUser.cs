using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mohamy.Core.Migrations
{
    /// <inheritdoc />
    public partial class updateLawyerUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experience_SubConsultings_subConsultingId",
                schema: "dbo",
                table: "Experience");

            migrationBuilder.DropForeignKey(
                name: "FK_Experience_Users_LawyerId",
                schema: "dbo",
                table: "Experience");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Experience",
                schema: "dbo",
                table: "Experience");

            migrationBuilder.RenameTable(
                name: "Experience",
                schema: "dbo",
                newName: "Experiences",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_Experience_subConsultingId",
                schema: "dbo",
                table: "Experiences",
                newName: "IX_Experiences_subConsultingId");

            migrationBuilder.RenameIndex(
                name: "IX_Experience_LawyerId",
                schema: "dbo",
                table: "Experiences",
                newName: "IX_Experiences_LawyerId");

            migrationBuilder.AddColumn<string>(
                name: "ImagesId",
                schema: "dbo",
                table: "Chats",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Experiences",
                schema: "dbo",
                table: "Experiences",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_ImagesId",
                schema: "dbo",
                table: "Chats",
                column: "ImagesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Images_ImagesId",
                schema: "dbo",
                table: "Chats",
                column: "ImagesId",
                principalSchema: "dbo",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Images_ImagesId",
                schema: "dbo",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_SubConsultings_subConsultingId",
                schema: "dbo",
                table: "Experiences");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_Users_LawyerId",
                schema: "dbo",
                table: "Experiences");

            migrationBuilder.DropIndex(
                name: "IX_Chats_ImagesId",
                schema: "dbo",
                table: "Chats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Experiences",
                schema: "dbo",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "ImagesId",
                schema: "dbo",
                table: "Chats");

            migrationBuilder.RenameTable(
                name: "Experiences",
                schema: "dbo",
                newName: "Experience",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_Experiences_subConsultingId",
                schema: "dbo",
                table: "Experience",
                newName: "IX_Experience_subConsultingId");

            migrationBuilder.RenameIndex(
                name: "IX_Experiences_LawyerId",
                schema: "dbo",
                table: "Experience",
                newName: "IX_Experience_LawyerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Experience",
                schema: "dbo",
                table: "Experience",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Experience_SubConsultings_subConsultingId",
                schema: "dbo",
                table: "Experience",
                column: "subConsultingId",
                principalSchema: "dbo",
                principalTable: "SubConsultings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Experience_Users_LawyerId",
                schema: "dbo",
                table: "Experience",
                column: "LawyerId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
