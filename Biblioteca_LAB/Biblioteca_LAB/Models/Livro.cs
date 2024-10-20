using System.ComponentModel.DataAnnotations;

namespace Biblioteca_LAB.Models
{
    public class Livro
    {
        [Key]
        [Required(ErrorMessage = "Required field")]
        public int ISBN { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [StringLength(100, ErrorMessage = "The {0} do not exceed {1} characteres.")]
        public string? Titulo { get; set; }
        [Required(ErrorMessage = "Required Field")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime Data_Edicao { get; set; }
        [Required(ErrorMessage = "Required Field")]
        public int num_exemplares { get; set; }

        public string? Capa_IMG { get; set; }
        [Required(ErrorMessage = "Required Field")]
        [StringLength(65535, ErrorMessage = "The {0} do not exceed {1} characteres.")]
        public string? Sinopse { get; set; }
    }
}
