namespace LibSpace_Aspnet.Models
{
    public class LivrosRequisitadosViewModel
    {
        public IEnumerable<Livro> PreRequisitados { get; set; } = new List<Livro>();
        public IEnumerable<RequisitadoInfo> Requisitados { get; set; } = new List<RequisitadoInfo>();
        public IEnumerable<EntregueInfo> Entregues { get; set; } = new List<EntregueInfo>();
    }

    public class RequisitadoInfo
    {
        public Livro Livro { get; set; }
        public DateTime DataRequisicao { get; set; }
        public DateOnly DataPrevEntrega { get; set; }
        public string BibliotecarioNome { get; set; }
    }

    public class EntregueInfo : RequisitadoInfo
    {
        public DateTime DataEntrega { get; set; }
        public string BibliotecarioRemetenteNome { get; set; }
    }
} 