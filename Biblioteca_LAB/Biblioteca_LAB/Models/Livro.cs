using System.ComponentModel.DataAnnotations;

namespace Biblioteca_LAB.Models
{
    public class Livro
    {
        [Key]
        [Required(ErrorMessage = "Required field")]
        public int ISBN { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateOnly Data_Edicao { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [StringLength(50, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? Idioma { get; set; }

        [Required(ErrorMessage = "Required Field")]
        public int num_exemplares { get; set; }

        // Optional field for the image of the book cover
        public string? Capa_IMG { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [StringLength(65535, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? Sinopse { get; set; }
    }
}
