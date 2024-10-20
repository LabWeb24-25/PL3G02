using System.ComponentModel.DataAnnotations;

namespace Biblioteca_LAB.Models
{
    public class Endereco
    {
        [Required(ErrorMessage = "Required Field")]
        [StringLength(100, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? Morada { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [StringLength(9, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        [RegularExpression(@"\d{4}-\d{3}", ErrorMessage = "The Postal Code must follow the format 'XXXX-XXX'.")]
        public string? CodigoPostal { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [StringLength(50, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? Localidade { get; set; }
    }
}
