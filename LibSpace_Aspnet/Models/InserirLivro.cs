using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibSpace_Aspnet.Models;

[Table("Inserir_Livro")]
public partial class InserirLivro
{
    [Key]
    [Column("ID_Livro")]
    public int IdLivro { get; set; }

    [Column("ID_Bibliotecario")]
    public int IdBibliotecario { get; set; }

    [ForeignKey("IdLivro")]
    [InverseProperty("InserirLivro")]
    public virtual Livro IdLivroNavigation { get; set; } = null!;
}
