using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBCreate.Models;

public partial class Genero
{
    [Key]
    [Column("ID_Generos")]
    public int IdGeneros { get; set; }

    [Column("Nome_Generos")]
    [StringLength(20)]
    [Unicode(false)]
    public string NomeGeneros { get; set; } = null!;

    [InverseProperty("IdGenerosNavigation")]
    public virtual ICollection<Livro> Livros { get; set; } = new List<Livro>();
}
