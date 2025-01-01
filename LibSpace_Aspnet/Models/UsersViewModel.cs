using Microsoft.AspNetCore.Identity;

namespace LibSpace_Aspnet.Models
{
    public class UsersViewModel
    {
        public IEnumerable<IdentityUser> AllUsers { get; set; }
        public IEnumerable<BibliotecarioPendente> PendingBibliotecarios { get; set; }
        public Dictionary<string, List<string>> UserRoles { get; set; }
        public string SelectedRole { get; set; }
        public bool? IsBlocked { get; set; }
        public bool IsAscending { get; set; } = true; // Default to ascending
    }
} 