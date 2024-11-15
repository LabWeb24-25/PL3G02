using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibSpace.Models;

[PrimaryKey("IdLeitor", "IdLivro", "DataRequisicao")]
public partial class Requisita
{
    [Key]
    [Column("ID_Leitor")]
    public int IdLeitor { get; set; }

    [Column("ID_BibliotecarioRecetor")]
    public int? IdBibliotecarioRecetor { get; set; }

    [Column("ID_BibliotecarioRemetente")]
    public int? IdBibliotecarioRemetente { get; set; }

    [Key]
    [Column("ID_Livro")]
    public int IdLivro { get; set; }

    [Key]
    [Column("Data_Requisicao", TypeName = "datetime")]
    public DateTime DataRequisicao { get; set; }

    [Column("Data_PrevEntrega")]
    public DateOnly DataPrevEntrega { get; set; }

    [Column("Data_Entrega", TypeName = "datetime")]
    public DateTime DataEntrega { get; set; }

    [ForeignKey("IdLivro")]
    [InverseProperty("Requisita")]
    public virtual Livro IdLivroNavigation { get; set; } = null!;

    [ForeignKey("IdBibliotecarioRecetor")]
    [InverseProperty("Requisita")]
    public virtual Livro IdBibliotecarioRecetorNavigation { get; set; } = null!;

    [ForeignKey("IdLeitor")]
    [InverseProperty("Requisita")]
    public virtual Perfil IdLeitorNavigation { get; set; } = null!;

    [ForeignKey("IdBibliotecarioRemetente")]
    [InverseProperty("Requisita")]
    public virtual Perfil IdBibliotecarioRemetenteNavigation { get; set; } = null!;
}
