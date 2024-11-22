using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibSpace.Models;

[Table("Autor")]
public class Autor
{
    [Key]
    [Column("ID_Autor")]
    public int IdAutor { get; set; }

    [Column("Nome_Autor")]
    [StringLength(120)]
    [Unicode(false)]
    public string NomeAutor { get; set; } = null!;

    [Column("Data_Nascimento")]
    public DateOnly DataNascimento { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Pseudonimo { get; set; }

    [Column("Data_Falecimento")]
    public DateOnly? DataFalecimento { get; set; }

    [Column("Foto_Autor")]
    [StringLength(250)]
    [Unicode(false)]
    public string FotoAutor { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Bibliografia { get; set; } = null!;

    [Column("Id_Lingua")]
    public int IdLingua { get; set; }

    [InverseProperty("IdAutorNavigation")]
    public virtual ICollection<Escreveu> Escreveus { get; set; } = new List<Escreveu>();

    [ForeignKey("IdLingua")]
    [InverseProperty("Autors")]
    public virtual Pais IdLinguaNavigation { get; set; } = null!;
}
