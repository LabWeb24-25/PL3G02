using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibSpace_Aspnet.Data;
using LibSpace_Aspnet.Models.ViewModels;

namespace LibSpace_Aspnet.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .Join(_context.Perfils,
                    user => user.Id,
                    perfil => perfil.AspNetUserId,
                    (user, perfil) => new UserListViewModel
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        Nome_Perfil = perfil.NomePerfil
                    })
                .ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> GetUserDetails(string id)
        {
            Console.WriteLine($"GetUserDetails called with id: {id}");
            
            var perfil = await _context.Perfils
                .FirstOrDefaultAsync(p => p.AspNetUserId == id);
            
            if (perfil == null)
            {
                Console.WriteLine($"No profile found for user id: {id}");
                return NotFound();
            }
            
            Console.WriteLine($"Found profile for user: {perfil.NomePerfil}");
            return PartialView("_UserDetailsModal", perfil);
        }
    }
}