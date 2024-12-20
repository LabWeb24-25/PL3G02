using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibSpace_Aspnet.Models
{
    public class EditoraViewModel
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(200, ErrorMessage = "O Nome não pode exceder {1} caracteres.")]
        public string NomeEditora { get; set; } = null!;

        [Required(ErrorMessage = "O campo Infomação de Editora é obrigatório.")]
        public string? InfoEditora { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Capa do Livro")]
        public IFormFile? ImgEditora { get; set; }

        public int stateimage { get; set; }
    }
}
