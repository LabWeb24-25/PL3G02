using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibSpace_Aspnet.Data.Migrations
{
    /// <inheritdoc />
    public partial class Updated_02_12_2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Clicks",
                table: "Livros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AspNetRoleAspNetUsers",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleAspNetUsers", x => new { x.RoleId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "PreRequisita",
                columns: table => new
                {
                    IDReserva = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDLeitor = table.Column<int>(type: "int", nullable: false),
                    IDLivro = table.Column<int>(type: "int", nullable: false),
                    EstadoLevantamento = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PreRequi__D9F2FA67713ECE76", x => x.IDReserva);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleAspNetUsers");

            migrationBuilder.DropTable(
                name: "PreRequisita");

            migrationBuilder.DropColumn(
                name: "Clicks",
                table: "Livros");
        }
    }
}
