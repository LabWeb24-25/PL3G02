using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibSpace_Aspnet.Data.Migrations
{
    /// <inheritdoc />
    public partial class BlockPropriedades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__Favorito__EA339903E35E92C3",
                table: "Favorito");

            migrationBuilder.AlterColumn<string>(
                name: "ID_Leitor",
                table: "Favorito",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Favorito__EA339903E35E92C3",
                table: "Favorito",
                columns: new[] { "ID_Leitor", "ID_Livro" });

            migrationBuilder.AlterColumn<string>(
                name: "ID_Admin",
                table: "Bloquear",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Data_Desbloqueio_Manual",
                table: "Bloquear",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Data_Fim_Bloqueio",
                table: "Bloquear",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<bool>(
                name: "Estado_Bloqueio",
                table: "Bloquear",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ID_Admin_Desbloqueio",
                table: "Bloquear",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bloquear_ID_Admin",
                table: "Bloquear",
                column: "ID_Admin");

            migrationBuilder.CreateIndex(
                name: "IX_Bloquear_ID_Admin_Desbloqueio",
                table: "Bloquear",
                column: "ID_Admin_Desbloqueio");

            migrationBuilder.AddForeignKey(
                name: "FK_Bloquear_AspNetUsers_ID_Admin",
                table: "Bloquear",
                column: "ID_Admin",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bloquear_AspNetUsers_ID_Admin_Desbloqueio",
                table: "Bloquear",
                column: "ID_Admin_Desbloqueio",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bloquear_AspNetUsers_ID_User",
                table: "Bloquear",
                column: "ID_User",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bloquear_AspNetUsers_ID_Admin",
                table: "Bloquear");

            migrationBuilder.DropForeignKey(
                name: "FK_Bloquear_AspNetUsers_ID_Admin_Desbloqueio",
                table: "Bloquear");

            migrationBuilder.DropForeignKey(
                name: "FK_Bloquear_AspNetUsers_ID_User",
                table: "Bloquear");

            migrationBuilder.DropIndex(
                name: "IX_Bloquear_ID_Admin",
                table: "Bloquear");

            migrationBuilder.DropIndex(
                name: "IX_Bloquear_ID_Admin_Desbloqueio",
                table: "Bloquear");

            migrationBuilder.DropColumn(
                name: "Data_Desbloqueio_Manual",
                table: "Bloquear");

            migrationBuilder.DropColumn(
                name: "Data_Fim_Bloqueio",
                table: "Bloquear");

            migrationBuilder.DropColumn(
                name: "Estado_Bloqueio",
                table: "Bloquear");

            migrationBuilder.DropColumn(
                name: "ID_Admin_Desbloqueio",
                table: "Bloquear");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Favorito__EA339903E35E92C3",
                table: "Favorito");

            migrationBuilder.AlterColumn<int>(
                name: "ID_Leitor",
                table: "Favorito",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Favorito__EA339903E35E92C3",
                table: "Favorito",
                columns: new[] { "ID_Leitor", "ID_Livro" });

            migrationBuilder.AlterColumn<string>(
                name: "ID_Admin",
                table: "Bloquear",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
