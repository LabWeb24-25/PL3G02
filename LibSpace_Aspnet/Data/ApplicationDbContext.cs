using System;
using System.Collections.Generic;
using LibSpace_Aspnet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace LibSpace_Aspnet.Data;

public partial class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

 

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

    public virtual DbSet<PreRequisitum> PreRequisita { get; set; }

    public virtual DbSet<Requisitum> Requisita { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=ApplicationDbContext");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Autor>(entity =>
        {
            entity.HasKey(e => e.IdAutor).HasName("PK__Autor__9626AD262656E632");

            entity.HasOne(d => d.IdLinguaNavigation).WithMany(p => p.Autors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Autor__Id_Lingua__6FE99F9F");
        });

        modelBuilder.Entity<Biblioteca>(entity =>
        {
            entity.HasKey(e => e.IdBiblioteca).HasName("PK__Bibliote__AC25616D9C304CDB");

            entity.Property(e => e.EndCodPostal).IsFixedLength();

            entity.HasOne(d => d.EndCodPostalNavigation).WithMany(p => p.Bibliotecas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bibliotec__End_C__60A75C0F");
        });

        modelBuilder.Entity<Bloquear>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__Bloquear__ED4DE4428EAD68ED");

            entity.Property(e => e.IdUser).ValueGeneratedNever();
        });

        modelBuilder.Entity<CodigoPostal>(entity =>
        {
            entity.HasKey(e => e.EndCodPostal).HasName("PK__CodigoPo__E00A98BC246C63B5");

            entity.Property(e => e.EndCodPostal).IsFixedLength();
        });

        modelBuilder.Entity<Editora>(entity =>
        {
            entity.HasKey(e => e.IdEditora).HasName("PK__Editora__478AEA9F17517426");
        });

        modelBuilder.Entity<Favorito>(entity =>
        {
            entity.HasKey(e => new { e.IdLivro, e.IdLeitor }).HasName("PK__Favorito__EA339903E35E92C3");

            entity.HasOne(d => d.IdLivroNavigation).WithMany(p => p.Favoritos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Favorito__ID_Liv__797309D9");
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.IdGeneros).HasName("PK__Generos__A73180FF3A28AF76");
        });

        modelBuilder.Entity<InserirLivro>(entity =>
        {
            entity.HasKey(e => e.IdLivro).HasName("PK__Inserir___B2E808E2205D1764");

            entity.Property(e => e.IdLivro).ValueGeneratedNever();

            entity.HasOne(d => d.IdLivroNavigation).WithOne(p => p.InserirLivro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inserir_L__ID_Li__7F2BE32F");
        });

        modelBuilder.Entity<Livro>(entity =>
        {
            entity.HasKey(e => e.IdLivro).HasName("PK__Livros__B2E808E26F44F882");

            entity.HasOne(d => d.IdAutorNavigation).WithMany(p => p.Livros)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Livros__ID_autor__73BA3083");

            entity.HasOne(d => d.IdEditoraNavigation).WithMany(p => p.Livros)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Livros__ID_Edito__72C60C4A");

            entity.HasOne(d => d.IdGenerosNavigation).WithMany(p => p.Livros)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Livros__ID_Gener__75A278F5");

            entity.HasOne(d => d.IdLinguaNavigation).WithMany(p => p.Livros)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Livros__ID_Lingu__74AE54BC");
        });

        modelBuilder.Entity<Pai>(entity =>
        {
            entity.HasKey(e => e.IdPais).HasName("PK__Pais__AE88C4EFF6827ECD");
        });

        modelBuilder.Entity<Perfil>(entity =>
        {
            entity.HasKey(e => e.IdPerfil).HasName("PK__Perfil__A3815057F71BD780");

            entity.Property(e => e.EndCodPostal).IsFixedLength();

            entity.HasOne(d => d.EndCodPostalNavigation).WithMany(p => p.Perfils)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Perfil__End_CodP__656C112C");
        });

        modelBuilder.Entity<PreRequisitum>(entity =>
        {
            entity.HasKey(e => e.Idreserva).HasName("PK__PreRequi__D9F2FA67713ECE76");
        });

        modelBuilder.Entity<Requisitum>(entity =>
        {
            entity.HasKey(e => new { e.IdLeitor, e.IdLivro, e.DataRequisicao }).HasName("PK__Requisit__945B6D7789B3AB0D");

            entity.HasOne(d => d.IdLivroNavigation).WithMany(p => p.Requisita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Requisita__ID_Li__7C4F7684");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
