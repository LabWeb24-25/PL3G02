using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibSpace.Models;

public partial class Genero
{
    [Key]
    [Column("ID_Generos")]
    public int IdGeneros { get; set; }

    [Column("Nome_Generos")]
    [StringLength(20)]
    [Unicode(false)]
    public string NomeGeneros { get; set; } = null!;

    [ForeignKey("IdGeneros")]
    [InverseProperty("IdGeneros")]
    public virtual ICollection<Livro> IdLivros { get; set; } = new List<Livro>();
}
