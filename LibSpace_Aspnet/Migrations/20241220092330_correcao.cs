using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibSpace_Aspnet.Data.Migrations
{
    /// <inheritdoc />
    public partial class correcao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "Clicks",
            //    table: "Livros",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);
            // First drop the primary key constraint
            migrationBuilder.DropPrimaryKey(
                name: "PK__Bloquear__ED4DE4428EAD68ED",
                table: "Bloquear");

            // Now modify the column
            migrationBuilder.AlterColumn<string>(
                name: "ID_User",
                table: "Bloquear",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // Re-add the primary key constraint
            migrationBuilder.AddPrimaryKey(
                name: "PK__Bloquear__ED4DE4428EAD68ED",
                table: "Bloquear",
                column: "ID_User");
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
              // Reverse the operations in case of rollback
            migrationBuilder.DropPrimaryKey(
                name: "PK__Bloquear__ED4DE4428EAD68ED",
                table: "Bloquear");

            migrationBuilder.AlterColumn<string>(
                name: "ID_User",
                table: "Bloquear",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Bloquear__ED4DE4428EAD68ED",
                table: "Bloquear",
                column: "ID_User");
            migrationBuilder.DropColumn(
                name: "Clicks",
                table: "Livros");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
