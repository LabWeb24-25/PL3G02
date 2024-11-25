using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBCreate.Models;

[Table("CodigoPostal")]
public partial class CodigoPostal
{
    [Key]
    [Column("End_CodPostal")]
    [StringLength(8)]
    [Unicode(false)]
    public string EndCodPostal { get; set; } = null!;

    [Column("End_Localidade")]
    [StringLength(40)]
    [Unicode(false)]
    public string EndLocalidade { get; set; } = null!;

    [InverseProperty("EndCodPostalNavigation")]
    public virtual ICollection<Biblioteca> Bibliotecas { get; set; } = new List<Biblioteca>();

    [InverseProperty("EndCodPostalNavigation")]
    public virtual ICollection<Perfil> Perfils { get; set; } = new List<Perfil>();
}
