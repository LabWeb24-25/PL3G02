﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibSpace.Models;

[Table("Bloquear")]
public partial class Bloquear
{
    [Column("ID_Admin")]
    public int IdAdmin { get; set; }

    [Key]
    [Column("ID_User")]
    public int IdUser { get; set; }

    [Column("Motivo_Bloquear", TypeName = "text")]
    public string MotivoBloquear { get; set; } = null!;

    [Column("Data_Bloqueio")]
    public DateOnly DataBloqueio { get; set; }

    [ForeignKey("IdAdmin")]
    [InverseProperty("Bloquear")]
    public virtual Perfil IdAdminNavigation { get; set; } = null!;

    [ForeignKey("IdUser")]
    [InverseProperty("Bloquear")]
    public virtual Perfil IdUserNavigation { get; set; } = null!;
}
