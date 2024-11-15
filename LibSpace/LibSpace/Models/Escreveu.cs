using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibSpace.Models;

[Table("Escreveu")]
public partial class Escreveu
{
    [Key]
    [Column("ID_Livro")]
    public int IdLivro { get; set; }

    [Column("ID_autor")]
    public int IdAutor { get; set; }

    [ForeignKey("IdAutor")]
    [InverseProperty("Escreveus")]
    public virtual Autor IdAutorNavigation { get; set; } = null!;

    [ForeignKey("IdLivro")]
    [InverseProperty("Escreveu")]
    public virtual Livro IdLivroNavigation { get; set; } = null!;
}
