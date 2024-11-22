using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibSpace.Models;

[Table("Publica")]
public class Publica
{
    [Key]
    [Column("ID_Livro")]
    public int IdLivro { get; set; }

    [Column("ID_Editora")]
    public int IdEditora { get; set; }

    [ForeignKey("IdEditora")]
    [InverseProperty("Publicas")]
    public virtual Editora IdEditoraNavigation { get; set; } = null!;

    [ForeignKey("IdLivro")]
    [InverseProperty("Publica")]
    public virtual Livro IdLivroNavigation { get; set; } = null!;
}
