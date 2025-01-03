using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibSpace_Aspnet.Data;
using LibSpace_Aspnet.Models;

namespace LibSpace_Aspnet.Controllers
{
    public class FavoritosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public FavoritosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var favoritos = await _context.Livros
                .Include(l => l.Favoritos)
                .Include(l => l.IdAutorNavigation)
                .Include(l => l.IdEditoraNavigation)
                .Where(l => l.Favoritos.Any(f => f.IdLeitor == user.Id))
                .ToListAsync();

            return View(favoritos);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFavorito(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var favoritoExistente = await _context.Favoritos
                .FirstOrDefaultAsync(f => f.IdLivro == id && f.IdLeitor == user.Id);

            if (favoritoExistente == null)
            {
                TempData["Error"] = "Livro não está nos favoritos.";
                return RedirectToAction(nameof(Index));
            }

            _context.Favoritos.Remove(favoritoExistente);
            await _context.SaveChangesAsync();
            
            TempData["Success"] = "Livro removido dos favoritos com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorito(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var favorito = new Favorito
            {
                IdLivro = id,
                IdLeitor = user.Id
            };

            _context.Favoritos.Add(favorito);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Livro adicionado aos favoritos com sucesso!";

            return RedirectToAction(nameof(Index));
        }
    }
} 