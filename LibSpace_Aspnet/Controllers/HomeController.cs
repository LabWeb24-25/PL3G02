using LibSpace_Aspnet.Data;
using LibSpace_Aspnet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using LibSpace_Aspnet.Data;

namespace LibSpace_Aspnet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 5; // Nº LIVROS POR SECCÇÃO AQUIII <-----------------

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1)
        {
            var totalBooks = _context.Livros.Count();
            var totalPages = (int)Math.Ceiling(totalBooks / (double)PageSize);

            var featuredBooks = _context.Livros
                .Include(l => l.IdAutorNavigation)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            
            return View(featuredBooks);
        }

        public async Task<IActionResult> Sobre()
        {
            var info = await _context.Bibliotecas.ToListAsync();
            return View(info);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
