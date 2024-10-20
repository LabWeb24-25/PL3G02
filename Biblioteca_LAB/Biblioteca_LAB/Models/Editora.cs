using System.ComponentModel.DataAnnotations;

namespace Biblioteca_LAB.Models
{
    public class Editora
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [StringLength(120, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? Nome { get; set; }

        // Info_Editora opciomal
        [StringLength(5000, ErrorMessage = "The Info cannot exceed {1} characters.")]
        public string? Info { get; set; }

        // Img_Editora opcional
        public string? IMG { get; set; }
    }
}
