using System.ComponentModel.DataAnnotations;

namespace Biblioteca_LAB.Models
{
    public class Perfil
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [StringLength(50, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [StringLength(50, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? Apelido { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateOnly DataNascimento { get; set; }

        public Endereco Endereco { get; set; } = new Endereco();
    }
}
