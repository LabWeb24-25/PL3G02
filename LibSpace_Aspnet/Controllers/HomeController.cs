using LibSpace_Aspnet.Data;
using LibSpace_Aspnet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using LibSpace_Aspnet.Data;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace LibSpace_Aspnet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 5; // N� LIVROS POR SECC��O AQUIII <-----------------
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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

            ViewBag.Generos = _context.Generos.ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            
            var announcementsPath = Path.Combine(_webHostEnvironment.WebRootPath, "img", "announcements");
            var imageFiles = Directory.GetFiles(announcementsPath)
                .Select(Path.GetFileName)
                .ToList();
            
            ViewBag.AnnouncementImages = imageFiles;
            
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
