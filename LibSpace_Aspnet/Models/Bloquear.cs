using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace LibSpace_Aspnet.Models;

[Table("Bloquear")]
public partial class Bloquear
{
    [Column("ID_Admin")]
    public string IdAdmin { get; set; } = null!;

    [Column("ID_User")]
    public string IdUser { get; set; } = null!;

    [Column("Motivo_Bloquear", TypeName = "text")]
    public string MotivoBloquear { get; set; } = null!;

    [Column("Data_Bloqueio", TypeName = "date")]
    public DateOnly DataBloqueio { get; set; }

    [Column("Data_Fim_Bloqueio", TypeName = "date")]
    public DateOnly DataFimBloqueio { get; set; }

    [Column("Estado_Bloqueio")]
    public bool EstadoBloqueio { get; set; }

    [Column("ID_Admin_Desbloqueio")]
    public string? IdAdminDesbloqueio { get; set; }

    [Column("Data_Desbloqueio_Manual", TypeName = "date")]
    public DateOnly? DataDesbloqueioManual { get; set; }

    [Key]
    [Column("ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdBloqueio { get; set; }

    // Navigation properties
    public virtual IdentityUser IdAdminNavigation { get; set; }
    public virtual IdentityUser IdUserNavigation { get; set; }
    public virtual IdentityUser IdAdminDesbloqueioNavigation { get; set; }
}
