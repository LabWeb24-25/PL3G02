using System.ComponentModel.DataAnnotations;

namespace Biblioteca_LAB.Models
{
    public class Biblioteca
    {
        [Key]
        public int Id_Biblioteca { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [StringLength(100, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string? Telefone { get; set; }

        [StringLength(100, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? Horario { get; set; }

        public Endereco Endereco { get; set; } = new Endereco();
    }
}
