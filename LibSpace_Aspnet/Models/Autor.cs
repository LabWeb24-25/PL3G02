using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibSpace_Aspnet.Models;

[Table("Autor")]
public partial class Autor
{
    [Key]
    [Column("ID_Autor")]
    public int IdAutor { get; set; }

    [Column("Nome_Autor")]
    [StringLength(120)]
    [Unicode(false)]
    public string NomeAutor { get; set; }

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
    public string? FotoAutor { get; set; }

    [Column(TypeName = "text")]
    public string Bibliografia { get; set; }

    [Column("Id_Lingua")]
    public int IdLingua { get; set; }

    [ForeignKey("IdLingua")]
    [InverseProperty("Autors")]
    public virtual Pai IdLinguaNavigation { get; set; }

    [InverseProperty("IdAutorNavigation")]
    public virtual ICollection<Livro> Livros { get; set; } = new List<Livro>();
}
