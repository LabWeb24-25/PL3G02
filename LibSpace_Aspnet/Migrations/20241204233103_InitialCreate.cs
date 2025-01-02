using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibSpace_Aspnet.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bloquear",
                columns: table => new
                {
                    ID_User = table.Column<int>(type: "int", nullable: false),
                    ID_Admin = table.Column<int>(type: "int", nullable: false),
                    Motivo_Bloquear = table.Column<string>(type: "text", nullable: false),
                    Data_Bloqueio = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Bloquear__ED4DE4428EAD68ED", x => x.ID_User);
                });

            migrationBuilder.CreateTable(
                name: "CodigoPostal",
                columns: table => new
                {
                    End_CodPostal = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false),
                    End_Localidade = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CodigoPo__E00A98BC246C63B5", x => x.End_CodPostal);
                });

            migrationBuilder.CreateTable(
                name: "Editora",
                columns: table => new
                {
                    ID_Editora = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome_Editora = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Info_Editora = table.Column<string>(type: "text", nullable: true),
                    Img_Editora = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Editora__478AEA9F17517426", x => x.ID_Editora);
                });

            migrationBuilder.CreateTable(
                name: "Generos",
                columns: table => new
                {
                    ID_Generos = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome_Generos = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Generos__A73180FF3A28AF76", x => x.ID_Generos);
                });

            migrationBuilder.CreateTable(
                name: "Pais",
                columns: table => new
                {
                    ID_Pais = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome_Pais = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Pais__AE88C4EFF6827ECD", x => x.ID_Pais);
                });

            migrationBuilder.CreateTable(
                name: "PendingBibliotecarios",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AspNetUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    AspNetUserIdAdmin = table.Column<string>(type: "nvarchar(450)") //adicionado à mão PODE DAR MERDA AQUI
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingBibliotecarios", x => x.ID);
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

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Biblioteca",
                columns: table => new
                {
                    Id_Biblioteca = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    End_Morada = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    End_CodPostal = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false),
                    Nome_Biblioteca = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar(125)", unicode: false, maxLength: 125, nullable: false),
                    Telefone = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: false),
                    Horario = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Bibliote__AC25616D9C304CDB", x => x.Id_Biblioteca);
                    table.ForeignKey(
                        name: "FK__Bibliotec__End_C__60A75C0F",
                        column: x => x.End_CodPostal,
                        principalTable: "CodigoPostal",
                        principalColumn: "End_CodPostal");
                });

            migrationBuilder.CreateTable(
                name: "Perfil",
                columns: table => new
                {
                    ID_perfil = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    End_Morada = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    End_CodPostal = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false),
                    Nome_Perfil = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    DataNascimento_Perfil = table.Column<DateOnly>(type: "date", nullable: false),
                    Apelido = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Img_Perfil = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    AspNetUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Perfil__A3815057F71BD780", x => x.ID_perfil);
                    table.ForeignKey(
                        name: "FK__Perfil__End_CodP__656C112C",
                        column: x => x.End_CodPostal,
                        principalTable: "CodigoPostal",
                        principalColumn: "End_CodPostal");
                });

            migrationBuilder.CreateTable(
                name: "Autor",
                columns: table => new
                {
                    ID_Autor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome_Autor = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false),
                    Data_Nascimento = table.Column<DateOnly>(type: "date", nullable: false),
                    Pseudonimo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Data_Falecimento = table.Column<DateOnly>(type: "date", nullable: true),
                    Foto_Autor = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    Bibliografia = table.Column<string>(type: "text", nullable: false),
                    Id_Lingua = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Autor__9626AD262656E632", x => x.ID_Autor);
                    table.ForeignKey(
                        name: "FK__Autor__Id_Lingua__6FE99F9F",
                        column: x => x.Id_Lingua,
                        principalTable: "Pais",
                        principalColumn: "ID_Pais");
                });

            migrationBuilder.CreateTable(
                name: "Livros",
                columns: table => new
                {
                    ID_Livro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ISBN = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    DataEdicao = table.Column<DateOnly>(type: "date", nullable: false),
                    Titulo_Livros = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Num_Exemplares = table.Column<int>(type: "int", nullable: false),
                    Capa_IMG = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    ID_Editora = table.Column<int>(type: "int", nullable: false),
                    ID_Lingua = table.Column<int>(type: "int", nullable: false),
                    Clicks = table.Column<int>(type: "int", nullable: false),
                    Sinopse = table.Column<string>(type: "text", nullable: false),
                    ID_autor = table.Column<int>(type: "int", nullable: false),
                    ID_Generos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Livros__B2E808E26F44F882", x => x.ID_Livro);
                    table.ForeignKey(
                        name: "FK__Livros__ID_Edito__72C60C4A",
                        column: x => x.ID_Editora,
                        principalTable: "Editora",
                        principalColumn: "ID_Editora");
                    table.ForeignKey(
                        name: "FK__Livros__ID_Gener__75A278F5",
                        column: x => x.ID_Generos,
                        principalTable: "Generos",
                        principalColumn: "ID_Generos");
                    table.ForeignKey(
                        name: "FK__Livros__ID_Lingu__74AE54BC",
                        column: x => x.ID_Lingua,
                        principalTable: "Pais",
                        principalColumn: "ID_Pais");
                    table.ForeignKey(
                        name: "FK__Livros__ID_autor__73BA3083",
                        column: x => x.ID_autor,
                        principalTable: "Autor",
                        principalColumn: "ID_Autor");
                });

            migrationBuilder.CreateTable(
                name: "Favorito",
                columns: table => new
                {
                    ID_Livro = table.Column<int>(type: "int", nullable: false),
                    ID_Leitor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Favorito__EA339903E35E92C3", x => new { x.ID_Livro, x.ID_Leitor });
                    table.ForeignKey(
                        name: "FK__Favorito__ID_Liv__797309D9",
                        column: x => x.ID_Livro,
                        principalTable: "Livros",
                        principalColumn: "ID_Livro");
                });

            migrationBuilder.CreateTable(
                name: "Inserir_Livro",
                columns: table => new
                {
                    ID_Livro = table.Column<int>(type: "int", nullable: false),
                    ID_Bibliotecario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Inserir___B2E808E2205D1764", x => x.ID_Livro);
                    table.ForeignKey(
                        name: "FK__Inserir_L__ID_Li__7F2BE32F",
                        column: x => x.ID_Livro,
                        principalTable: "Livros",
                        principalColumn: "ID_Livro");
                });

            migrationBuilder.CreateTable(
                name: "Requisita",
                columns: table => new
                {
                    ID_Leitor = table.Column<int>(type: "int", nullable: false),
                    ID_Livro = table.Column<int>(type: "int", nullable: false),
                    Data_Requisicao = table.Column<DateTime>(type: "datetime", nullable: false),
                    ID_BibliotecarioRecetor = table.Column<int>(type: "int", nullable: false),
                    ID_BibliotecarioRemetente = table.Column<int>(type: "int", nullable: true),
                    Data_PrevEntrega = table.Column<DateOnly>(type: "date", nullable: false),
                    Data_Entrega = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Requisit__945B6D7789B3AB0D", x => new { x.ID_Leitor, x.ID_Livro, x.Data_Requisicao });
                    table.ForeignKey(
                        name: "FK__Requisita__ID_Li__7C4F7684",
                        column: x => x.ID_Livro,
                        principalTable: "Livros",
                        principalColumn: "ID_Livro");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Autor_Id_Lingua",
                table: "Autor",
                column: "Id_Lingua");

            migrationBuilder.CreateIndex(
                name: "IX_Biblioteca_End_CodPostal",
                table: "Biblioteca",
                column: "End_CodPostal");

            migrationBuilder.CreateIndex(
                name: "IX_Livros_ID_autor",
                table: "Livros",
                column: "ID_autor");

            migrationBuilder.CreateIndex(
                name: "IX_Livros_ID_Editora",
                table: "Livros",
                column: "ID_Editora");

            migrationBuilder.CreateIndex(
                name: "IX_Livros_ID_Generos",
                table: "Livros",
                column: "ID_Generos");

            migrationBuilder.CreateIndex(
                name: "IX_Livros_ID_Lingua",
                table: "Livros",
                column: "ID_Lingua");

            migrationBuilder.CreateIndex(
                name: "IX_Perfil_End_CodPostal",
                table: "Perfil",
                column: "End_CodPostal");

            migrationBuilder.CreateIndex(
                name: "IX_Requisita_ID_Livro",
                table: "Requisita",
                column: "ID_Livro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Biblioteca");

            migrationBuilder.DropTable(
                name: "Bloquear");

            migrationBuilder.DropTable(
                name: "Favorito");

            migrationBuilder.DropTable(
                name: "Inserir_Livro");

            migrationBuilder.DropTable(
                name: "PendingBibliotecarios");

            migrationBuilder.DropTable(
                name: "Perfil");

            migrationBuilder.DropTable(
                name: "PreRequisita");

            migrationBuilder.DropTable(
                name: "Requisita");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CodigoPostal");

            migrationBuilder.DropTable(
                name: "Livros");

            migrationBuilder.DropTable(
                name: "Editora");

            migrationBuilder.DropTable(
                name: "Generos");

            migrationBuilder.DropTable(
                name: "Autor");

            migrationBuilder.DropTable(
                name: "Pais");
        }
    }
}
