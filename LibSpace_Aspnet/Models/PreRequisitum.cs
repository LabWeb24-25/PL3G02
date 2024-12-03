using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibSpace_Aspnet.Models;

public partial class PreRequisitum
{
    [Key]
    [Column("IDReserva")]
    public int Idreserva { get; set; }

    [Column("IDLeitor")]
    public int Idleitor { get; set; }

    [Column("IDLivro")]
    public int Idlivro { get; set; }

    public int EstadoLevantamento { get; set; }
}
