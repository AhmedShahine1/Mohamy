﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mohamy.Core.Migrations
{
    /// <inheritdoc />
    public partial class ImageFieldNotRquiredChatTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Images_ImagesId",
                schema: "dbo",
                table: "Chats");

            migrationBuilder.AlterColumn<string>(
                name: "ImagesId",
                schema: "dbo",
                table: "Chats",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Images_ImagesId",
                schema: "dbo",
                table: "Chats",
                column: "ImagesId",
                principalSchema: "dbo",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Images_ImagesId",
                schema: "dbo",
                table: "Chats");

            migrationBuilder.AlterColumn<string>(
                name: "ImagesId",
                schema: "dbo",
                table: "Chats",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Images_ImagesId",
                schema: "dbo",
                table: "Chats",
                column: "ImagesId",
                principalSchema: "dbo",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
