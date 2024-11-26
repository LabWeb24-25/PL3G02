using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibSpace_Aspnet.Models;

[Table("Perfil")]
public partial class Perfil
{
    [Key]
    [Column("ID_perfil")]
    public int IdPerfil { get; set; }

    [Column("End_Morada")]
    [StringLength(100)]
    [Unicode(false)]
    public string EndMorada { get; set; } = null!;

    [Column("End_CodPostal")]
    [StringLength(8)]
    [Unicode(false)]
    public string EndCodPostal { get; set; } = null!;

    [Column("Nome_Perfil")]
    [StringLength(50)]
    [Unicode(false)]
    public string NomePerfil { get; set; } = null!;

    [Column("DataNascimento_Perfil")]
    public DateOnly DataNascimentoPerfil { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Apelido { get; set; } = null!;

    [Column("Img_Perfil")]
    [StringLength(300)]
    [Unicode(false)]
    public string ImgPerfil { get; set; } = null!;

    [StringLength(450)]
    public string AspNetUserId { get; set; } = null!;

    [ForeignKey("EndCodPostal")]
    [InverseProperty("Perfils")]
    public virtual CodigoPostal EndCodPostalNavigation { get; set; } = null!;
}
