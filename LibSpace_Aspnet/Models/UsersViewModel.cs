using Microsoft.AspNetCore.Identity;

namespace LibSpace_Aspnet.Models
{
    public class UsersViewModel
    {
        public IEnumerable<BibliotecarioPendente> PendingBibliotecarios { get; set; }
        public IEnumerable<IdentityUser> AllUsers { get; set; }

        // Dictionary where key = userId, value = list of roles
        public Dictionary<string, List<string>> UserRoles { get; set; }
    }
} 