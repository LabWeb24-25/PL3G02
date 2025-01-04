using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibSpace_Aspnet.Data.Migrations
{
    /// <inheritdoc />
    public partial class ID_Bloquear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "Bloquear",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            // Drop the existing primary key
            migrationBuilder.DropPrimaryKey(
                name: "PK__Bloquear__ED4DE4428EAD68ED",
                table: "Bloquear");

            // Add new primary key
            migrationBuilder.AddPrimaryKey(
                name: "PK__Bloquear__ED4DE4428EAD68ED",
                table: "Bloquear",
                column: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert primary key back to ID_User
            migrationBuilder.DropPrimaryKey(
                name: "PK__Bloquear__ED4DE4428EAD68ED",
                table: "Bloquear");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Bloquear__ED4DE4428EAD68ED",
                table: "Bloquear",
                column: "ID_User");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "Bloquear");
        }
    }
}
