using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibSpace.Models;

public partial class Pai
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
}
