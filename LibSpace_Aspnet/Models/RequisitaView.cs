namespace LibSpace_Aspnet.Models
{
    public class RequisitaView
    {
        public IEnumerable<RequisitaViewModel> Requisita { get; set; }
        public IEnumerable<PreRequisitaViewModel> PreRequisita { get; set; }
        public int? SelectedStatus { get; set; }
    }
}
