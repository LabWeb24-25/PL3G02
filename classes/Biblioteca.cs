using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBCreate.Models;

[Table("Biblioteca")]
public partial class Biblioteca
{
    [Key]
    [Column("Id_Biblioteca")]
    public int IdBiblioteca { get; set; }

    [Column("End_Morada")]
    [StringLength(250)]
    [Unicode(false)]
    public string EndMorada { get; set; } = null!;

    [Column("End_CodPostal")]
    [StringLength(8)]
    [Unicode(false)]
    public string EndCodPostal { get; set; } = null!;

    [Column("Nome_Biblioteca")]
    [StringLength(100)]
    [Unicode(false)]
    public string NomeBiblioteca { get; set; } = null!;

    [StringLength(125)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(9)]
    [Unicode(false)]
    public string Telefone { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Horario { get; set; } = null!;

    [ForeignKey("EndCodPostal")]
    [InverseProperty("Bibliotecas")]
    public virtual CodigoPostal EndCodPostalNavigation { get; set; } = null!;
}
