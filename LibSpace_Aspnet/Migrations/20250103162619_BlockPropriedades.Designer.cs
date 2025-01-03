﻿// <auto-generated />
using System;
using LibSpace_Aspnet.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LibSpace_Aspnet.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250103162619_BlockPropriedades")]
    partial class BlockPropriedades
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LibSpace_Aspnet.Models.Autor", b =>
                {
                    b.Property<int>("IdAutor")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID_Autor");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdAutor"));

                    b.Property<string>("Bibliografia")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateOnly?>("DataFalecimento")
                        .HasColumnType("date")
                        .HasColumnName("Data_Falecimento");

                    b.Property<DateOnly>("DataNascimento")
                        .HasColumnType("date")
                        .HasColumnName("Data_Nascimento");

                    b.Property<string>("FotoAutor")
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("Foto_Autor");

                    b.Property<int>("IdLingua")
                        .HasColumnType("int")
                        .HasColumnName("Id_Lingua");

                    b.Property<string>("NomeAutor")
                        .IsRequired()
                        .HasMaxLength(120)
                        .IsUnicode(false)
                        .HasColumnType("varchar(120)")
                        .HasColumnName("Nome_Autor");

                    b.Property<string>("Pseudonimo")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("IdAutor")
                        .HasName("PK__Autor__9626AD262656E632");

                    b.HasIndex("IdLingua");

                    b.ToTable("Autor");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.Biblioteca", b =>
                {
                    b.Property<int>("IdBiblioteca")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Biblioteca");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdBiblioteca"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(125)
                        .IsUnicode(false)
                        .HasColumnType("varchar(125)");

                    b.Property<string>("EndCodPostal")
                        .IsRequired()
                        .HasMaxLength(8)
                        .IsUnicode(false)
                        .HasColumnType("char(8)")
                        .HasColumnName("End_CodPostal")
                        .IsFixedLength();

                    b.Property<string>("EndMorada")
                        .IsRequired()
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("End_Morada");

                    b.Property<string>("Horario")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NomeBiblioteca")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("Nome_Biblioteca");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasMaxLength(9)
                        .IsUnicode(false)
                        .HasColumnType("varchar(9)");

                    b.HasKey("IdBiblioteca")
                        .HasName("PK__Bibliote__AC25616D9C304CDB");

                    b.HasIndex("EndCodPostal");

                    b.ToTable("Biblioteca");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.BibliotecarioPendente", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("ApplicationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("AspNetUserId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AspNetUserIdAdmin")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.HasKey("ID");

                    b.HasIndex("AspNetUserId");

                    b.HasIndex("AspNetUserIdAdmin");

                    b.ToTable("PendingBibliotecarios");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.Bloquear", b =>
                {
                    b.Property<string>("IdUser")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("ID_User");

                    b.Property<DateOnly>("DataBloqueio")
                        .HasColumnType("date")
                        .HasColumnName("Data_Bloqueio");

                    b.Property<DateOnly?>("DataDesbloqueioManual")
                        .HasColumnType("date")
                        .HasColumnName("Data_Desbloqueio_Manual");

                    b.Property<DateOnly>("DataFimBloqueio")
                        .HasColumnType("date")
                        .HasColumnName("Data_Fim_Bloqueio");

                    b.Property<bool>("EstadoBloqueio")
                        .HasColumnType("bit")
                        .HasColumnName("Estado_Bloqueio");

                    b.Property<string>("IdAdmin")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("ID_Admin");

                    b.Property<string>("IdAdminDesbloqueio")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("ID_Admin_Desbloqueio");

                    b.Property<string>("MotivoBloquear")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Motivo_Bloquear");

                    b.HasKey("IdUser")
                        .HasName("PK__Bloquear__ED4DE4428EAD68ED");

                    b.HasIndex("IdAdmin");

                    b.HasIndex("IdAdminDesbloqueio");

                    b.ToTable("Bloquear");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.CodigoPostal", b =>
                {
                    b.Property<string>("EndCodPostal")
                        .HasMaxLength(8)
                        .IsUnicode(false)
                        .HasColumnType("char(8)")
                        .HasColumnName("End_CodPostal")
                        .IsFixedLength();

                    b.Property<string>("EndLocalidade")
                        .IsRequired()
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("varchar(40)")
                        .HasColumnName("End_Localidade");

                    b.HasKey("EndCodPostal")
                        .HasName("PK__CodigoPo__E00A98BC246C63B5");

                    b.ToTable("CodigoPostal");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.Editora", b =>
                {
                    b.Property<int>("IdEditora")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID_Editora");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdEditora"));

                    b.Property<string>("ImgEditora")
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("varchar(300)")
                        .HasColumnName("Img_Editora");

                    b.Property<string>("InfoEditora")
                        .HasColumnType("text")
                        .HasColumnName("Info_Editora");

                    b.Property<string>("NomeEditora")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("Nome_Editora");

                    b.HasKey("IdEditora")
                        .HasName("PK__Editora__478AEA9F17517426");

                    b.ToTable("Editora");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.Favorito", b =>
                {
                    b.Property<int>("IdLivro")
                        .HasColumnType("int")
                        .HasColumnName("ID_Livro");

                    b.Property<string>("IdLeitor")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("ID_Leitor");

                    b.HasKey("IdLivro", "IdLeitor")
                        .HasName("PK__Favorito__EA339903E35E92C3");

                    b.ToTable("Favorito");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.Genero", b =>
                {
                    b.Property<int>("IdGeneros")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID_Generos");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdGeneros"));

                    b.Property<string>("NomeGeneros")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("Nome_Generos");

                    b.HasKey("IdGeneros")
                        .HasName("PK__Generos__A73180FF3A28AF76");

                    b.ToTable("Generos");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.InserirLivro", b =>
                {
                    b.Property<int>("IdLivro")
                        .HasColumnType("int")
                        .HasColumnName("ID_Livro");

                    b.Property<int>("IdBibliotecario")
                        .HasColumnType("int")
                        .HasColumnName("ID_Bibliotecario");

                    b.HasKey("IdLivro")
                        .HasName("PK__Inserir___B2E808E2205D1764");

                    b.ToTable("Inserir_Livro");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.Livro", b =>
                {
                    b.Property<int>("IdLivro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID_Livro");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdLivro"));

                    b.Property<string>("CapaImg")
                        .IsRequired()
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("Capa_IMG");

                    b.Property<int>("Clicks")
                        .HasColumnType("int")
                        .HasColumnName("Clicks");

                    b.Property<DateOnly>("DataEdicao")
                        .HasColumnType("date");

                    b.Property<int>("IdAutor")
                        .HasColumnType("int")
                        .HasColumnName("ID_autor");

                    b.Property<int>("IdEditora")
                        .HasColumnType("int")
                        .HasColumnName("ID_Editora");

                    b.Property<int>("IdGeneros")
                        .HasColumnType("int")
                        .HasColumnName("ID_Generos");

                    b.Property<int>("IdLingua")
                        .HasColumnType("int")
                        .HasColumnName("ID_Lingua");

                    b.Property<string>("Isbn")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("ISBN");

                    b.Property<int>("NumExemplares")
                        .HasColumnType("int")
                        .HasColumnName("Num_Exemplares");

                    b.Property<string>("Sinopse")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TituloLivros")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("Titulo_Livros");

                    b.HasKey("IdLivro")
                        .HasName("PK__Livros__B2E808E26F44F882");

                    b.HasIndex("IdAutor");

                    b.HasIndex("IdEditora");

                    b.HasIndex("IdGeneros");

                    b.HasIndex("IdLingua");

                    b.ToTable("Livros");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.Pai", b =>
                {
                    b.Property<int>("IdPais")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID_Pais");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPais"));

                    b.Property<string>("NomePais")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("Nome_Pais");

                    b.HasKey("IdPais")
                        .HasName("PK__Pais__AE88C4EFF6827ECD");

                    b.ToTable("Pais");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.Perfil", b =>
                {
                    b.Property<int>("IdPerfil")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID_perfil");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPerfil"));

                    b.Property<string>("Apelido")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("AspNetUserId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateOnly>("DataNascimentoPerfil")
                        .HasColumnType("date")
                        .HasColumnName("DataNascimento_Perfil");

                    b.Property<string>("EndCodPostal")
                        .IsRequired()
                        .HasMaxLength(8)
                        .IsUnicode(false)
                        .HasColumnType("char(8)")
                        .HasColumnName("End_CodPostal")
                        .IsFixedLength();

                    b.Property<string>("EndMorada")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("End_Morada");

                    b.Property<string>("ImgPerfil")
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("varchar(300)")
                        .HasColumnName("Img_Perfil");

                    b.Property<string>("NomePerfil")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("Nome_Perfil");

                    b.HasKey("IdPerfil")
                        .HasName("PK__Perfil__A3815057F71BD780");

                    b.HasIndex("EndCodPostal");

                    b.ToTable("Perfil");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.PreRequisitum", b =>
                {
                    b.Property<int>("Idreserva")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IDReserva");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Idreserva"));

                    b.Property<int>("EstadoLevantamento")
                        .HasColumnType("int");

                    b.Property<int>("Idleitor")
                        .HasColumnType("int")
                        .HasColumnName("IDLeitor");

                    b.Property<int>("Idlivro")
                        .HasColumnType("int")
                        .HasColumnName("IDLivro");

                    b.HasKey("Idreserva")
                        .HasName("PK__PreRequi__D9F2FA67713ECE76");

                    b.ToTable("PreRequisita");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.Requisitum", b =>
                {
                    b.Property<int>("IdLeitor")
                        .HasColumnType("int")
                        .HasColumnName("ID_Leitor");

                    b.Property<int>("IdLivro")
                        .HasColumnType("int")
                        .HasColumnName("ID_Livro");

                    b.Property<DateTime>("DataRequisicao")
                        .HasColumnType("datetime")
                        .HasColumnName("Data_Requisicao");

                    b.Property<DateTime?>("DataEntrega")
                        .HasColumnType("datetime")
                        .HasColumnName("Data_Entrega");

                    b.Property<DateOnly>("DataPrevEntrega")
                        .HasColumnType("date")
                        .HasColumnName("Data_PrevEntrega");

                    b.Property<int>("IdBibliotecarioRecetor")
                        .HasColumnType("int")
                        .HasColumnName("ID_BibliotecarioRecetor");

                    b.Property<int?>("IdBibliotecarioRemetente")
                        .HasColumnType("int")
                        .HasColumnName("ID_BibliotecarioRemetente");

                    b.HasKey("IdLeitor", "IdLivro", "DataRequisicao")
                        .HasName("PK__Requisit__945B6D7789B3AB0D");

                    b.HasIndex("IdLivro");

                    b.ToTable("Requisita");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.Autor", b =>
                {
                    b.HasOne("LibSpace_Aspnet.Models.Pai", "IdLinguaNavigation")
                        .WithMany("Autors")
                        .HasForeignKey("IdLingua")
                        .IsRequired()
                        .HasConstraintName("FK__Autor__Id_Lingua__6FE99F9F");

                    b.Navigation("IdLinguaNavigation");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.Biblioteca", b =>
                {
                    b.HasOne("LibSpace_Aspnet.Models.CodigoPostal", "EndCodPostalNavigation")
                        .WithMany("Bibliotecas")
                        .HasForeignKey("EndCodPostal")
                        .IsRequired()
                        .HasConstraintName("FK__Bibliotec__End_C__60A75C0F");

                    b.Navigation("EndCodPostalNavigation");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.BibliotecarioPendente", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "AspNetUser")
                        .WithMany()
                        .HasForeignKey("AspNetUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "AspNetUserIdAdminNavigation")
                        .WithMany()
                        .HasForeignKey("AspNetUserIdAdmin");

                    b.Navigation("AspNetUser");

                    b.Navigation("AspNetUserIdAdminNavigation");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.Bloquear", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "IdAdminNavigation")
                        .WithMany()
                        .HasForeignKey("IdAdmin")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "IdAdminDesbloqueioNavigation")
                        .WithMany()
                        .HasForeignKey("IdAdminDesbloqueio")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "IdUserNavigation")
                        .WithMany()
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IdAdminDesbloqueioNavigation");

                    b.Navigation("IdAdminNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.Favorito", b =>
                {
                    b.HasOne("LibSpace_Aspnet.Models.Livro", "IdLivroNavigation")
                        .WithMany("Favoritos")
                        .HasForeignKey("IdLivro")
                        .IsRequired()
                        .HasConstraintName("FK__Favorito__ID_Liv__797309D9");

                    b.Navigation("IdLivroNavigation");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.InserirLivro", b =>
                {
                    b.HasOne("LibSpace_Aspnet.Models.Livro", "IdLivroNavigation")
                        .WithOne("InserirLivro")
                        .HasForeignKey("LibSpace_Aspnet.Models.InserirLivro", "IdLivro")
                        .IsRequired()
                        .HasConstraintName("FK__Inserir_L__ID_Li__7F2BE32F");

                    b.Navigation("IdLivroNavigation");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.Livro", b =>
                {
                    b.HasOne("LibSpace_Aspnet.Models.Autor", "IdAutorNavigation")
                        .WithMany("Livros")
                        .HasForeignKey("IdAutor")
                        .IsRequired()
                        .HasConstraintName("FK__Livros__ID_autor__73BA3083");

                    b.HasOne("LibSpace_Aspnet.Models.Editora", "IdEditoraNavigation")
                        .WithMany("Livros")
                        .HasForeignKey("IdEditora")
                        .IsRequired()
                        .HasConstraintName("FK__Livros__ID_Edito__72C60C4A");

                    b.HasOne("LibSpace_Aspnet.Models.Genero", "IdGenerosNavigation")
                        .WithMany("Livros")
                        .HasForeignKey("IdGeneros")
                        .IsRequired()
                        .HasConstraintName("FK__Livros__ID_Gener__75A278F5");

                    b.HasOne("LibSpace_Aspnet.Models.Pai", "IdLinguaNavigation")
                        .WithMany("Livros")
                        .HasForeignKey("IdLingua")
                        .IsRequired()
                        .HasConstraintName("FK__Livros__ID_Lingu__74AE54BC");

                    b.Navigation("IdAutorNavigation");

                    b.Navigation("IdEditoraNavigation");

                    b.Navigation("IdGenerosNavigation");

                    b.Navigation("IdLinguaNavigation");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.Perfil", b =>
                {
                    b.HasOne("LibSpace_Aspnet.Models.CodigoPostal", "EndCodPostalNavigation")
                        .WithMany("Perfils")
                        .HasForeignKey("EndCodPostal")
                        .IsRequired()
                        .HasConstraintName("FK__Perfil__End_CodP__656C112C");

                    b.Navigation("EndCodPostalNavigation");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.Requisitum", b =>
                {
                    b.HasOne("LibSpace_Aspnet.Models.Livro", "IdLivroNavigation")
                        .WithMany("Requisita")
                        .HasForeignKey("IdLivro")
                        .IsRequired()
                        .HasConstraintName("FK__Requisita__ID_Li__7C4F7684");

                    b.Navigation("IdLivroNavigation");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.Autor", b =>
                {
                    b.Navigation("Livros");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.CodigoPostal", b =>
                {
                    b.Navigation("Bibliotecas");

                    b.Navigation("Perfils");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.Editora", b =>
                {
                    b.Navigation("Livros");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.Genero", b =>
                {
                    b.Navigation("Livros");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.Livro", b =>
                {
                    b.Navigation("Favoritos");

                    b.Navigation("InserirLivro");

                    b.Navigation("Requisita");
                });

            modelBuilder.Entity("LibSpace_Aspnet.Models.Pai", b =>
                {
                    b.Navigation("Autors");

                    b.Navigation("Livros");
                });
#pragma warning restore 612, 618
        }
    }
}
