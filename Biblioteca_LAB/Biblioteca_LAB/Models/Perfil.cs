using System.ComponentModel.DataAnnotations;

namespace Biblioteca_LAB.Models
{
    public class Perfil
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage ="Required Field")]
        [StringLength(50, ErrorMessage = "The {0} do not exceed {1} characteres.")]
        public string? Nome { get; set; }
        [Required(ErrorMessage = "Required Field")]
        [StringLength(50, ErrorMessage = "The {0} do not exceed {1} characteres.")]
        public string? Apelido { get; set; }
        [Required(ErrorMessage = "Required Field")]
        [StringLength(100, ErrorMessage = "The {0} do not exceed {1} characteres.")]
        public string? End_Morada { get; set; }
        [Required(ErrorMessage = "Required Field")]
        [StringLength(9, ErrorMessage = "The {0} do not exceed {1} characteres.")]
        [RegularExpression(@"\d{4}-\d{3}", ErrorMessage = "The Postal Code must follow the format 'XXXX-XXX'.")]
        public string? End_CodPostal { get; set; }
        [Required(ErrorMessage = "Required Field")]
        [StringLength(50, ErrorMessage = "The {0} do not exceed {1} characteres.")]
        public string? End_Localidade { get; set; }
        [Required(ErrorMessage = "Required Field")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime DataNascimento { get; set; }
        public string? Imagem { get; set; }

    }
}
