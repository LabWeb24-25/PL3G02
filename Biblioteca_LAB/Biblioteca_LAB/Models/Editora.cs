using System.ComponentModel.DataAnnotations;

namespace Biblioteca_LAB.Models
{
    public class Editora
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Required Field")]
        [StringLength(120, ErrorMessage = "The {0} do not exceed {1} characteres.")]
        public string? Nome { get; set; }
        [StringLength(5000, ErrorMessage = "The Info do not exceed {1} characteres.")]
        public string? Info { get; set; }
        public string? IMG { get; set; }
    }
}
