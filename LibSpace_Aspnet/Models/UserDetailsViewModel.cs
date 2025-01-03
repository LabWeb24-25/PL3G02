namespace LibSpace_Aspnet.Models
{
    public class UserDetailsViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Nome { get; set; }
        public string Apelido { get; set; }
        public string Morada { get; set; }
        public string CodigoPostal { get; set; }
        public string Localidade { get; set; }
        public string DataNascimento { get; set; }
        public string Role { get; set; }
        public string ImgPerfil { get; set; }
        public string? AdminApproverName { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public List<BanInfo> BanHistory { get; set; } = new List<BanInfo>();
        public bool IsCurrentlyBanned { get; set; }
        public BanInfo? CurrentBan { get; set; }
    }

    public class BanInfo
    {
        public string AdminName { get; set; }
        public string UnbanAdminName { get; set; }
        public string Reason { get; set; }
        public DateOnly BanDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool IsActive { get; set; }
        public DateOnly? ManualUnbanDate { get; set; }
    }
} 