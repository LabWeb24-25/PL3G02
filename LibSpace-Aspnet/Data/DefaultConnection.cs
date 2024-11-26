using System;
using System.Collections.Generic;
using LibSpace_Aspnet.Models;
using Microsoft.EntityFrameworkCore;

namespace LibSpace_Aspnet.Data;

public partial class DefaultConnection : DbContext
{
    public DefaultConnection()
    {
    }

    public DefaultConnection(DbContextOptions<DefaultConnection> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Autor> Autors { get; set; }

    public virtual DbSet<Biblioteca> Bibliotecas { get; set; }

    public virtual DbSet<Bloquear> Bloquears { get; set; }

    public virtual DbSet<CodigoPostal> CodigoPostals { get; set; }

    public virtual DbSet<Editora> Editoras { get; set; }

    public virtual DbSet<Favorito> Favoritos { get; set; }

    public virtual DbSet<Genero> Generos { get; set; }

    public virtual DbSet<InserirLivro> InserirLivros { get; set; }

    public virtual DbSet<Livro> Livros { get; set; }

    public virtual DbSet<Pai> Pais { get; set; }

    public virtual DbSet<Perfil> Perfils { get; set; }

    public virtual DbSet<Requisitum> Requisita { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<Autor>(entity =>
        {
            entity.HasKey(e => e.IdAutor).HasName("PK__Autor__9626AD26C156ECB9");

            entity.HasOne(d => d.IdLinguaNavigation).WithMany(p => p.Autors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Autor__Id_Lingua__5AEE82B9");
        });

        modelBuilder.Entity<Biblioteca>(entity =>
        {
            entity.HasKey(e => e.IdBiblioteca).HasName("PK__Bibliote__AC25616DAD2D5081");

            entity.Property(e => e.EndCodPostal).IsFixedLength();

            entity.HasOne(d => d.EndCodPostalNavigation).WithMany(p => p.Bibliotecas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bibliotec__End_C__4AB81AF0");
        });

        modelBuilder.Entity<Bloquear>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__Bloquear__ED4DE442AD408A71");

            entity.Property(e => e.IdUser).ValueGeneratedNever();
        });

        modelBuilder.Entity<CodigoPostal>(entity =>
        {
            entity.HasKey(e => e.EndCodPostal).HasName("PK__CodigoPo__E00A98BC3D444EE0");

            entity.Property(e => e.EndCodPostal).IsFixedLength();
        });

        modelBuilder.Entity<Editora>(entity =>
        {
            entity.HasKey(e => e.IdEditora).HasName("PK__Editora__478AEA9F830204E7");
        });

        modelBuilder.Entity<Favorito>(entity =>
        {
            entity.HasKey(e => new { e.IdLivro, e.IdLeitor }).HasName("PK__Favorito__EA3399033C905062");

            entity.HasOne(d => d.IdLivroNavigation).WithMany(p => p.Favoritos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Favorito__ID_Liv__6477ECF3");
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.IdGeneros).HasName("PK__Generos__A73180FFFD61615A");
        });

        modelBuilder.Entity<InserirLivro>(entity =>
        {
            entity.HasKey(e => e.IdLivro).HasName("PK__Inserir___B2E808E25F982BC2");

            entity.Property(e => e.IdLivro).ValueGeneratedNever();

            entity.HasOne(d => d.IdLivroNavigation).WithOne(p => p.InserirLivro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inserir_L__ID_Li__6A30C649");
        });

        modelBuilder.Entity<Livro>(entity =>
        {
            entity.HasKey(e => e.IdLivro).HasName("PK__Livros__B2E808E223B8A1DD");

            entity.HasOne(d => d.IdAutorNavigation).WithMany(p => p.Livros)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Livros__ID_autor__5EBF139D");

            entity.HasOne(d => d.IdEditoraNavigation).WithMany(p => p.Livros)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Livros__ID_Edito__5DCAEF64");

            entity.HasOne(d => d.IdGenerosNavigation).WithMany(p => p.Livros)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Livros__ID_Gener__60A75C0F");

            entity.HasOne(d => d.IdLinguaNavigation).WithMany(p => p.Livros)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Livros__ID_Lingu__5FB337D6");
        });

        modelBuilder.Entity<Pai>(entity =>
        {
            entity.HasKey(e => e.IdPais).HasName("PK__Pais__AE88C4EF1D979C14");
        });

        modelBuilder.Entity<Perfil>(entity =>
        {
            entity.HasKey(e => e.IdPerfil).HasName("PK__Perfil__A38150577F1BD7BF");

            entity.Property(e => e.EndCodPostal).IsFixedLength();

            entity.HasOne(d => d.AspNetUser).WithMany(p => p.Perfils)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Perfil__AspNetUs__5070F446");

            entity.HasOne(d => d.EndCodPostalNavigation).WithMany(p => p.Perfils)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Perfil__End_CodP__4F7CD00D");
        });

        modelBuilder.Entity<Requisitum>(entity =>
        {
            entity.HasKey(e => new { e.IdLeitor, e.IdLivro, e.DataRequisicao }).HasName("PK__Requisit__945B6D770B1D3023");

            entity.HasOne(d => d.IdLivroNavigation).WithMany(p => p.Requisita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Requisita__ID_Li__6754599E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
