using System;
using System.Collections.Generic;
using LibSpace_Aspnet.Models;
using Microsoft.EntityFrameworkCore;

namespace LibSpace_Aspnet.Data;

public partial class LibSpace_AspnetContext : DbContext
{
    public LibSpace_AspnetContext()
    {
    }

    public LibSpace_AspnetContext(DbContextOptions<LibSpace_AspnetContext> options)
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

    public virtual DbSet<Requisitum> Requisita { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=LibSpace_AspnetContext");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autor>(entity =>
        {
            entity.HasKey(e => e.IdAutor).HasName("PK__Autor__9626AD269DD234D9");

            entity.HasOne(d => d.IdLinguaNavigation).WithMany(p => p.Autors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Autor__Id_Lingua__37A5467C");
        });

        modelBuilder.Entity<Biblioteca>(entity =>
        {
            entity.HasKey(e => e.IdBiblioteca).HasName("PK__Bibliote__AC25616D67BFDFF9");

            entity.Property(e => e.EndCodPostal).IsFixedLength();

            entity.HasOne(d => d.EndCodPostalNavigation).WithMany(p => p.Bibliotecas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bibliotec__End_C__286302EC");
        });

        modelBuilder.Entity<Bloquear>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__Bloquear__ED4DE442A43B88D8");

            entity.Property(e => e.IdUser).ValueGeneratedNever();
        });

        modelBuilder.Entity<CodigoPostal>(entity =>
        {
            entity.HasKey(e => e.EndCodPostal).HasName("PK__CodigoPo__E00A98BCDFAF0FD5");

            entity.Property(e => e.EndCodPostal).IsFixedLength();
        });

        modelBuilder.Entity<Editora>(entity =>
        {
            entity.HasKey(e => e.IdEditora).HasName("PK__Editora__478AEA9FE708D26E");
        });

        modelBuilder.Entity<Favorito>(entity =>
        {
            entity.HasKey(e => new { e.IdLivro, e.IdLeitor }).HasName("PK__Favorito__EA339903D9A8B088");

            entity.HasOne(d => d.IdLivroNavigation).WithMany(p => p.Favoritos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Favorito__ID_Liv__412EB0B6");
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.IdGeneros).HasName("PK__Generos__A73180FF93F6E96F");
        });

        modelBuilder.Entity<InserirLivro>(entity =>
        {
            entity.HasKey(e => e.IdLivro).HasName("PK__Inserir___B2E808E2161C436B");

            entity.Property(e => e.IdLivro).ValueGeneratedNever();

            entity.HasOne(d => d.IdLivroNavigation).WithOne(p => p.InserirLivro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inserir_L__ID_Li__46E78A0C");
        });

        modelBuilder.Entity<Livro>(entity =>
        {
            entity.HasKey(e => e.IdLivro).HasName("PK__Livros__B2E808E2E16E401F");

            entity.HasOne(d => d.IdAutorNavigation).WithMany(p => p.Livros)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Livros__ID_autor__3B75D760");

            entity.HasOne(d => d.IdEditoraNavigation).WithMany(p => p.Livros)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Livros__ID_Edito__3A81B327");

            entity.HasOne(d => d.IdGenerosNavigation).WithMany(p => p.Livros)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Livros__ID_Gener__3D5E1FD2");

            entity.HasOne(d => d.IdLinguaNavigation).WithMany(p => p.Livros)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Livros__ID_Lingu__3C69FB99");
        });

        modelBuilder.Entity<Pai>(entity =>
        {
            entity.HasKey(e => e.IdPais).HasName("PK__Pais__AE88C4EF0FD6BEEE");
        });

        modelBuilder.Entity<Perfil>(entity =>
        {
            entity.HasKey(e => e.IdPerfil).HasName("PK__Perfil__A3815057CBB43EA9");

            entity.Property(e => e.EndCodPostal).IsFixedLength();

            entity.HasOne(d => d.EndCodPostalNavigation).WithMany(p => p.Perfils)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Perfil__End_CodP__2D27B809");
        });

        modelBuilder.Entity<Requisitum>(entity =>
        {
            entity.HasKey(e => new { e.IdLeitor, e.IdLivro, e.DataRequisicao }).HasName("PK__Requisit__945B6D77A0164940");

            entity.HasOne(d => d.IdLivroNavigation).WithMany(p => p.Requisita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Requisita__ID_Li__440B1D61");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
