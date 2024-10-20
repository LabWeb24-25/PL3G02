using System.ComponentModel.DataAnnotations;

namespace Biblioteca_LAB.Models
{
    public class Autor
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Required Field")]
        [StringLength(200, ErrorMessage = "The {0} do not exceed {1} characteres.")]
        public string? Nome { get; set; }
        [Required(ErrorMessage = "Required Field")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime DataNascimento { get; set; }
        [Required(ErrorMessage = "Required Field")]
        [StringLength(50, ErrorMessage = "The {0} do not exceed {1} characteres.")]
        public string? Pseudonimo {  get; set; }
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime DataFalecimento { get; set; }
        public string? Foto { get; set; }
        [Required(ErrorMessage = "Required Field")]
        [StringLength(10000, ErrorMessage = "The Bibliografia do not exceed {1} characteres.")]
        public string? Bibliografia { get; set; }
    }
}
