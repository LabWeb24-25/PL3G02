using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBCreate.Models;

public partial class Livro
{
    [Key]
    [Column("ID_Livro")]
    public int IdLivro { get; set; }

    [Column("ISBN")]
    [StringLength(20)]
    [Unicode(false)]
    public string Isbn { get; set; } = null!;

    public DateOnly DataEdicao { get; set; }

    [Column("Titulo_Livros")]
    [StringLength(100)]
    [Unicode(false)]
    public string TituloLivros { get; set; } = null!;

    [Column("Num_Exemplares")]
    public int NumExemplares { get; set; }

    [Column("Capa_IMG")]
    [StringLength(250)]
    [Unicode(false)]
    public string CapaImg { get; set; } = null!;

    [Column("ID_Editora")]
    public int IdEditora { get; set; }

    [Column("ID_Lingua")]
    public int IdLingua { get; set; }

    [Column(TypeName = "text")]
    public string Sinopse { get; set; } = null!;

    [Column("ID_autor")]
    public int IdAutor { get; set; }

    [Column("ID_Generos")]
    public int IdGeneros { get; set; }

    [InverseProperty("IdLivroNavigation")]
    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();

    [ForeignKey("IdAutor")]
    [InverseProperty("Livros")]
    public virtual Autor IdAutorNavigation { get; set; } = null!;

    [ForeignKey("IdEditora")]
    [InverseProperty("Livros")]
    public virtual Editora IdEditoraNavigation { get; set; } = null!;

    [ForeignKey("IdGeneros")]
    [InverseProperty("Livros")]
    public virtual Genero IdGenerosNavigation { get; set; } = null!;

    [ForeignKey("IdLingua")]
    [InverseProperty("Livros")]
    public virtual Pais IdLinguaNavigation { get; set; } = null!;

    [InverseProperty("IdLivroNavigation")]
    public virtual InserirLivro? InserirLivro { get; set; }

    [InverseProperty("IdLivroNavigation")]
    public virtual ICollection<Requisita> Requisita { get; set; } = new List<Requisita>();
}
