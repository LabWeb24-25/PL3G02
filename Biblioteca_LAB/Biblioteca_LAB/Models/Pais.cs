using System.ComponentModel.DataAnnotations;

namespace Biblioteca_LAB.Models
{
    public class Pais
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Required Field")]
        [StringLength(50, ErrorMessage = "The {0} cannot exceed {1} characteres.")]
        public string? Nome { get; set; }
    }
}
