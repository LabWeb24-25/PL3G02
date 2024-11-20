using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibSpace.Data.Migrations
{
    /// <inheritdoc />
    public partial class Autor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bloquear",
                columns: table => new
                {
                    ID_User = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Admin = table.Column<int>(type: "int", nullable: false),
                    Motivo_Bloquear = table.Column<string>(type: "text", nullable: false),
                    Data_Bloqueio = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bloquear", x => x.ID_User);
                });

            migrationBuilder.CreateTable(
                name: "CodigoPostal",
                columns: table => new
                {
                    End_CodPostal = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    End_Localidade = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodigoPostal", x => x.End_CodPostal);
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
                    table.PrimaryKey("PK_Editora", x => x.ID_Editora);
                });

            migrationBuilder.CreateTable(
                name: "Genero",
                columns: table => new
                {
                    ID_Generos = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome_Generos = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genero", x => x.ID_Generos);
                });

            migrationBuilder.CreateTable(
                name: "Livro",
                columns: table => new
                {
                    ID_Livro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ISBN = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    DataEdicao = table.Column<DateOnly>(type: "date", nullable: false),
                    Titulo_Livros = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Idioma = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Num_Exemplares = table.Column<int>(type: "int", nullable: false),
                    Capa_IMG = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    Sinopse = table.Column<string>(type: "text", nullable: false),
                    Clicks = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livro", x => x.ID_Livro);
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
                    table.PrimaryKey("PK_Pais", x => x.ID_Pais);
                });

            migrationBuilder.CreateTable(
                name: "Biblioteca",
                columns: table => new
                {
                    Id_Biblioteca = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    End_Morada = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    End_CodPostal = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    Nome_Biblioteca = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar(125)", unicode: false, maxLength: 125, nullable: false),
                    Telefone = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: false),
                    Horario = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biblioteca", x => x.Id_Biblioteca);
                    table.ForeignKey(
                        name: "FK_Biblioteca_CodigoPostal_End_CodPostal",
                        column: x => x.End_CodPostal,
                        principalTable: "CodigoPostal",
                        principalColumn: "End_CodPostal",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Perfil",
                columns: table => new
                {
                    ID_perfil = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    End_Morada = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    End_CodPostal = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    Nome_Perfil = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    DataNascimento_Perfil = table.Column<DateOnly>(type: "date", nullable: false),
                    Apelido = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Img_Perfil = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: false),
                    Username = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfil", x => x.ID_perfil);
                    table.ForeignKey(
                        name: "FK_Perfil_CodigoPostal_End_CodPostal",
                        column: x => x.End_CodPostal,
                        principalTable: "CodigoPostal",
                        principalColumn: "End_CodPostal",
                        onDelete: ReferentialAction.Cascade);
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
                    table.PrimaryKey("PK_Favorito", x => new { x.ID_Livro, x.ID_Leitor });
                    table.ForeignKey(
                        name: "FK_Favorito_Livro_ID_Livro",
                        column: x => x.ID_Livro,
                        principalTable: "Livro",
                        principalColumn: "ID_Livro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneroLivro",
                columns: table => new
                {
                    IdGeneros = table.Column<int>(type: "int", nullable: false),
                    IdLivro = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneroLivro", x => new { x.IdGeneros, x.IdLivro });
                    table.ForeignKey(
                        name: "FK_GeneroLivro_Genero_IdGeneros",
                        column: x => x.IdGeneros,
                        principalTable: "Genero",
                        principalColumn: "ID_Generos",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneroLivro_Livro_IdLivro",
                        column: x => x.IdLivro,
                        principalTable: "Livro",
                        principalColumn: "ID_Livro",
                        onDelete: ReferentialAction.Cascade);
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
                    table.PrimaryKey("PK_Inserir_Livro", x => x.ID_Livro);
                    table.ForeignKey(
                        name: "FK_Inserir_Livro_Livro_ID_Livro",
                        column: x => x.ID_Livro,
                        principalTable: "Livro",
                        principalColumn: "ID_Livro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Publica",
                columns: table => new
                {
                    ID_Livro = table.Column<int>(type: "int", nullable: false),
                    ID_Editora = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publica", x => x.ID_Livro);
                    table.ForeignKey(
                        name: "FK_Publica_Editora_ID_Editora",
                        column: x => x.ID_Editora,
                        principalTable: "Editora",
                        principalColumn: "ID_Editora",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Publica_Livro_ID_Livro",
                        column: x => x.ID_Livro,
                        principalTable: "Livro",
                        principalColumn: "ID_Livro",
                        onDelete: ReferentialAction.Cascade);
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
                    table.PrimaryKey("PK_Requisita", x => new { x.ID_Leitor, x.ID_Livro, x.Data_Requisicao });
                    table.ForeignKey(
                        name: "FK_Requisita_Livro_ID_Livro",
                        column: x => x.ID_Livro,
                        principalTable: "Livro",
                        principalColumn: "ID_Livro",
                        onDelete: ReferentialAction.Cascade);
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
                    table.PrimaryKey("PK_Autor", x => x.ID_Autor);
                    table.ForeignKey(
                        name: "FK_Autor_Pais_Id_Lingua",
                        column: x => x.Id_Lingua,
                        principalTable: "Pais",
                        principalColumn: "ID_Pais",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Escreveu",
                columns: table => new
                {
                    ID_Livro = table.Column<int>(type: "int", nullable: false),
                    ID_autor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escreveu", x => x.ID_Livro);
                    table.ForeignKey(
                        name: "FK_Escreveu_Autor_ID_autor",
                        column: x => x.ID_autor,
                        principalTable: "Autor",
                        principalColumn: "ID_Autor",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Escreveu_Livro_ID_Livro",
                        column: x => x.ID_Livro,
                        principalTable: "Livro",
                        principalColumn: "ID_Livro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Autor_Id_Lingua",
                table: "Autor",
                column: "Id_Lingua");

            migrationBuilder.CreateIndex(
                name: "IX_Biblioteca_End_CodPostal",
                table: "Biblioteca",
                column: "End_CodPostal");

            migrationBuilder.CreateIndex(
                name: "IX_Escreveu_ID_autor",
                table: "Escreveu",
                column: "ID_autor");

            migrationBuilder.CreateIndex(
                name: "IX_GeneroLivro_IdLivro",
                table: "GeneroLivro",
                column: "IdLivro");

            migrationBuilder.CreateIndex(
                name: "IX_Perfil_End_CodPostal",
                table: "Perfil",
                column: "End_CodPostal");

            migrationBuilder.CreateIndex(
                name: "IX_Publica_ID_Editora",
                table: "Publica",
                column: "ID_Editora");

            migrationBuilder.CreateIndex(
                name: "IX_Requisita_ID_Livro",
                table: "Requisita",
                column: "ID_Livro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Biblioteca");

            migrationBuilder.DropTable(
                name: "Bloquear");

            migrationBuilder.DropTable(
                name: "Escreveu");

            migrationBuilder.DropTable(
                name: "Favorito");

            migrationBuilder.DropTable(
                name: "GeneroLivro");

            migrationBuilder.DropTable(
                name: "Inserir_Livro");

            migrationBuilder.DropTable(
                name: "Perfil");

            migrationBuilder.DropTable(
                name: "Publica");

            migrationBuilder.DropTable(
                name: "Requisita");

            migrationBuilder.DropTable(
                name: "Autor");

            migrationBuilder.DropTable(
                name: "Genero");

            migrationBuilder.DropTable(
                name: "CodigoPostal");

            migrationBuilder.DropTable(
                name: "Editora");

            migrationBuilder.DropTable(
                name: "Livro");

            migrationBuilder.DropTable(
                name: "Pais");
        }
    }
}
