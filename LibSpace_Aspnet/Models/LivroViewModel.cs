using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LibSpace.Models
{
    public class LivroViewModel
    {
        [Required(ErrorMessage = "O campo ISBN é obrigatório.")]
        [StringLength(20, ErrorMessage = "O ISBN não pode exceder {1} caracteres.")]
        public string Isbn { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Título é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Título não pode exceder {1} caracteres.")]
        public string TituloLivros { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Autor é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Autor não pode exceder {1} caracteres.")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Editora é obrigatório.")]
        [StringLength(100, ErrorMessage = "A Editora não pode exceder {1} caracteres.")]
        public string Editora { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Data de Edição é obrigatório.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Edição")]
        public DateTime DataEdicao { get; set; }

        [Required(ErrorMessage = "O campo Idioma é obrigatório.")]
        [StringLength(50, ErrorMessage = "O Idioma não pode exceder {1} caracteres.")]
        public string Idioma { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Número de Exemplares é obrigatório.")]
        [Range(0, int.MaxValue, ErrorMessage = "O Número de Exemplares deve ser um valor não negativo.")]
        [Display(Name = "Número de Exemplares em Stock")]
        public int NumExemplares { get; set; }

        [Required(ErrorMessage = "Selecione uma imagem para a capa.")]
        [DataType(DataType.Upload)]
        [Display(Name = "Capa do Livro")]
        public IFormFile? CapaImg { get; set; }

        [Required(ErrorMessage = "O campo Sinopse é obrigatório.")]
        [DataType(DataType.MultilineText)]
        public string Sinopse { get; set; } = string.Empty;
    }
}
