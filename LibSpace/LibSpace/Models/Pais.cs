using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBCreate.Models;

public partial class Pais
{
    [Key]
    [Column("ID_Pais")]
    public int IdPais { get; set; }

    [Column("Nome_Pais")]
    [StringLength(50)]
    [Unicode(false)]
    public string NomePais { get; set; } = null!;

    [InverseProperty("IdLinguaNavigation")]
    public virtual ICollection<Autor> Autors { get; set; } = new List<Autor>();

    [InverseProperty("IdLinguaNavigation")]
    public virtual ICollection<Livro> Livros { get; set; } = new List<Livro>();
}
