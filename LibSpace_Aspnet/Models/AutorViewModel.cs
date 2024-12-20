using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LibSpace_Aspnet.Models
{
    public class AutorViewModel
    {
        [Required(ErrorMessage = "O campo Nome do Autor é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Nome do Autor não pode exceder {1} caracteres.")]
        [Display(Name = "Nome do Autor")]
        public string NomeAutor { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Data de Nascimento é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        public DateOnly DataNascimento { get; set; }

        [StringLength(100, ErrorMessage = "O Pseudônimo não pode exceder {1} caracteres.")]
        [Display(Name = "Pseudônimo")]
        public string? Pseudonimo { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Falecimento")]
        public DateOnly? DataFalecimento { get; set; }

        [Display(Name = "Foto do Autor")]
        public IFormFile? FotoAutor { get; set; }

        [Required(ErrorMessage = "A Bibliografia é obrigatória.")]
        [StringLength(1000, ErrorMessage = "A Bibliografia não pode exceder {1} caracteres.")]
        [Display(Name = "Bibliografia")]
        public string Bibliografia { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Idioma é obrigatório.")]
        [Display(Name = "Idioma")]
        public int IdLingua { get; set; }

        public int statusimg { get; set; }
    }
}
