using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibSpace_Aspnet.Models;

[Table("Editora")]
public partial class Editora
{
    [Key]
    [Column("ID_Editora")]
    public int IdEditora { get; set; }

    [Column("Nome_Editora")]
    [StringLength(100)]
    [Unicode(false)]
    public string NomeEditora { get; set; } = null!;

    [Column("Info_Editora", TypeName = "text")]
    public string? InfoEditora { get; set; }

    [Column("Img_Editora")]
    [StringLength(300)]
    [Unicode(false)]
    public string? ImgEditora { get; set; } = null!;

    [InverseProperty("IdEditoraNavigation")]
    public virtual ICollection<Livro> Livros { get; set; } = new List<Livro>();
}
