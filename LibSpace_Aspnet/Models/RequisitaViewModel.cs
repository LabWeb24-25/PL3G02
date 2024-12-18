namespace LibSpace_Aspnet.Models
{
    public class RequisitaViewModel
    {
        public int IdReserva { get; set; }
        public string NomeLivro { get; set; }
        public string NomeLeitor { get; set; }
        public DateTime DataRequisicao { get; set; }
        public DateOnly DataPrevEntrega { get; set; }
        public DateTime DataEntrega { get; set; }
        public string NomeBibliotecarioRecetor { get; set; }
        public string NomeBibliotecarioRemetente { get; set; }
    }
}
