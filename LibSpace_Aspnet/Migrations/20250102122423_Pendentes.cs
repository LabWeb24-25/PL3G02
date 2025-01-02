using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibSpace_Aspnet.Data.Migrations
{
    /// <inheritdoc />
    public partial class Pendentes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Data_Entrega",
                table: "Requisita",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AddColumn<string>(
                name: "AspNetUserIdAdmin",
                table: "PendingBibliotecarios",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Img_Editora",
                table: "Editora",
                type: "varchar(300)",
                unicode: false,
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(300)",
                oldUnicode: false,
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "ID_Admin",
                table: "Bloquear",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ID_User",
                table: "Bloquear",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Foto_Autor",
                table: "Autor",
                type: "varchar(250)",
                unicode: false,
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldUnicode: false,
                oldMaxLength: 250);

            migrationBuilder.CreateIndex(
                name: "IX_PendingBibliotecarios_AspNetUserId",
                table: "PendingBibliotecarios",
                column: "AspNetUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PendingBibliotecarios_AspNetUsers_AspNetUserId",
                table: "PendingBibliotecarios",
                column: "AspNetUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PendingBibliotecarios_AspNetUsers_AspNetUserId",
                table: "PendingBibliotecarios");

            migrationBuilder.DropIndex(
                name: "IX_PendingBibliotecarios_AspNetUserId",
                table: "PendingBibliotecarios");

            migrationBuilder.DropColumn(
                name: "AspNetUserIdAdmin",
                table: "PendingBibliotecarios");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Data_Entrega",
                table: "Requisita",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Img_Editora",
                table: "Editora",
                type: "varchar(300)",
                unicode: false,
                maxLength: 300,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(300)",
                oldUnicode: false,
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_Admin",
                table: "Bloquear",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ID_User",
                table: "Bloquear",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Foto_Autor",
                table: "Autor",
                type: "varchar(250)",
                unicode: false,
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldUnicode: false,
                oldMaxLength: 250,
                oldNullable: true);
        }
    }
}
