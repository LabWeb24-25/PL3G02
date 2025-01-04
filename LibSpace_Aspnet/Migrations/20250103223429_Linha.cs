using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibSpace_Aspnet.Data.Migrations
{
    /// <inheritdoc />
    public partial class Linha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Bibliotec__End_C__60A75C0F",
                table: "Biblioteca");

            migrationBuilder.DropIndex(
                name: "IX_Biblioteca_End_CodPostal",
                table: "Biblioteca");

            migrationBuilder.AlterColumn<string>(
                name: "End_CodPostal",
                table: "Biblioteca",
                type: "varchar(8)",
                unicode: false,
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(8)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 8);

            migrationBuilder.AddColumn<string>(
                name: "End_Localidade",
                table: "Biblioteca",
                type: "varchar(40)",
                unicode: false,
                maxLength: 40,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "End_Localidade",
                table: "Biblioteca");

            migrationBuilder.AlterColumn<string>(
                name: "End_CodPostal",
                table: "Biblioteca",
                type: "char(8)",
                unicode: false,
                fixedLength: true,
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(8)",
                oldUnicode: false,
                oldMaxLength: 8);

            migrationBuilder.CreateIndex(
                name: "IX_Biblioteca_End_CodPostal",
                table: "Biblioteca",
                column: "End_CodPostal");

            migrationBuilder.AddForeignKey(
                name: "FK__Bibliotec__End_C__60A75C0F",
                table: "Biblioteca",
                column: "End_CodPostal",
                principalTable: "CodigoPostal",
                principalColumn: "End_CodPostal");
        }
    }
}
