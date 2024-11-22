using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibSpace.Models;

[PrimaryKey("IdLivro", "IdLeitor")]
[Table("Favorito")]
public class Favorito
{
    [Key]
    [Column("ID_Livro")]
    public int IdLivro { get; set; }

    [Key]
    [Column("ID_Leitor")]
    public int IdLeitor { get; set; }

    [ForeignKey("IdLivro")]
    [InverseProperty("Favoritos")]
    public virtual Livro IdLivroNavigation { get; set; } = null!;
}
