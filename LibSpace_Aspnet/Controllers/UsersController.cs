using LibSpace_Aspnet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibSpace_Aspnet.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        public IActionResult Index()
        {
            return View();
        }
        // GETamos: Autors/Perfil
        public async Task<IActionResult> Perfil(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autors
                .Include(a => a.IdLinguaNavigation)
                .FirstOrDefaultAsync(m => m.IdAutor == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }
    }
}