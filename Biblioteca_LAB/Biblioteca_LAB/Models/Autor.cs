using System.ComponentModel.DataAnnotations;

namespace Biblioteca_LAB.Models
{
    public class Autor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [StringLength(200, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [StringLength(200, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? Nacionalidade { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateOnly DataNascimento { get; set; }

        [StringLength(50, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? Pseudonimo { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateOnly? DataFalecimento { get; set; }

        // Foto pode ser um URL
        public string? Foto { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [StringLength(10000, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? Bibliografia { get; set; }
    }
}
