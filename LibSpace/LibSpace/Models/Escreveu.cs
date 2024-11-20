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
<<<<<<< HEAD
    [InverseProperty("Autor")]
=======
    [InverseProperty("Escreveus")]
>>>>>>> parent of 18577d2 (Up Models)
    public virtual Autor IdAutorNavigation { get; set; } = null!;

    [ForeignKey("IdLivro")]
    [InverseProperty("Livro")]
    public virtual Livro IdLivroNavigation { get; set; } = null!;
}
