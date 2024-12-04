using Microsoft.AspNetCore.Identity;

namespace LibSpace_Aspnet.Data
{
    public class SeedRoles
    {

        public static void Seed(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                roleManager.CreateAsync(new IdentityRole("Leitor")).Wait();
                roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
                roleManager.CreateAsync(new IdentityRole("Bibliotecario")).Wait();

            }
        }

    }
}
