using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibSpace.Models;

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

    [StringLength(50)]
    [Unicode(false)]
    public string Idioma { get; set; } = null!;

    [Column("Num_Exemplares")]
    public int NumExemplares { get; set; }

    [Column("Capa_IMG")]
    [StringLength(250)]
    [Unicode(false)]
    public string CapaImg { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Sinopse { get; set; } = null!;

    [InverseProperty("IdLivroNavigation")]
    public virtual Escreveu? Escreveu { get; set; }

    [InverseProperty("IdLivroNavigation")]
    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();

    [InverseProperty("IdLivroNavigation")]
    public virtual InserirLivro? InserirLivro { get; set; }

    [InverseProperty("IdLivroNavigation")]
    public virtual Publica? Publica { get; set; }

    [InverseProperty("IdLivroNavigation")]
    public virtual ICollection<Requisitum> Requisita { get; set; } = new List<Requisitum>();

    [ForeignKey("IdLivro")]
    [InverseProperty("IdLivros")]
    public virtual ICollection<Genero> IdGeneros { get; set; } = new List<Genero>();
}
