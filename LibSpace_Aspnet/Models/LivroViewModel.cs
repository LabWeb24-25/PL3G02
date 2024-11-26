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

        // Substituir a propriedade string Autor por IdAutor para refletir o dropdown
        [Required(ErrorMessage = "O campo Autor é obrigatório.")]
        [Display(Name = "Autor")]
        public int IdAutor { get; set; }

        // Substituir a propriedade string Editora por IdEditora para refletir o dropdown
        [Required(ErrorMessage = "O campo Editora é obrigatório.")]
        [Display(Name = "Editora")]
        public int IdEditora { get; set; }

        // Substituir a propriedade string Idioma por IdLingua para refletir o dropdown
        [Required(ErrorMessage = "O campo Idioma é obrigatório.")]
        [Display(Name = "País de Origem")]
        public int IdLingua { get; set; }

        // Adicionar a propriedade IdGeneros para refletir o dropdown de Gêneros
        [Required(ErrorMessage = "O campo Gênero é obrigatório.")]
        [Display(Name = "Gênero")]
        public int IdGeneros { get; set; }

        [Required(ErrorMessage = "O campo Data de Edição é obrigatório.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Publicação")]
        public DateTime DataEdicao { get; set; }

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
