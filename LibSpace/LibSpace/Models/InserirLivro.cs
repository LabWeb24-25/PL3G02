using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibSpace.Models;

[Table("Inserir_Livro")]
public class InserirLivro
{
    [Column("ID_Bibliotecario")]
    public int IdBibliotecario { get; set; }

    [Key]
    [Column("ID_Livro")]
    public int IdLivro { get; set; }

    [ForeignKey("IdLivro")]
    [InverseProperty("InserirLivro")]
    public virtual Livro IdLivroNavigation { get; set; } = null!;
}
