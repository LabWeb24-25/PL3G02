using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibSpace_Aspnet.Models;

[Table("Bloquear")]
public partial class Bloquear
{
    [Column("ID_Admin")]
    public string IdAdmin { get; set; } = null!;

    [Key]
    [Column("ID_User")]
    public string IdUser { get; set; } = null!;

    [Column("Motivo_Bloquear", TypeName = "text")]
    public string MotivoBloquear { get; set; } = null!;

    [Column("Data_Bloqueio")]
    public DateOnly DataBloqueio { get; set; }

    [Column("Data_Fim_Bloqueio")]
    public DateOnly DataFimBloqueio { get; set; }

    [Column("Estado_Bloqueio")]
    public bool EstadoBloqueio { get; set; }

    //Allow Nulls
    [Column("ID_Admin_Desbloqueio")]
    public string? IdAdminDesbloqueio { get; set; }

    [Column("Data_Desbloqueio_Manual")]
    public DateOnly? DataDesbloqueioManual { get; set; }
}
