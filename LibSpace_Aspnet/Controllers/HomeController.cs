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
        private const int PageSize = 6; // livros por linha AQUIII <-----------------
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(int page = 1, string section = "featured", int? genreId = null)
        {
            // Featured books query
            var featuredQuery = _context.Livros
                .Include(l => l.IdAutorNavigation)
                .Include(l => l.IdGenerosNavigation)
                .Include(l => l.IdEditoraNavigation);
            
            var featuredTotalBooks = featuredQuery.Count();
            var featuredTotalPages = (int)Math.Ceiling(featuredTotalBooks / (double)PageSize);

            var featuredBooks = featuredQuery
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            // Get all genres and their books with independent pagination
            var genres = _context.Generos.ToList();
            var genreBooks = new Dictionary<int, List<Livro>>();
            var genrePages = new Dictionary<int, int>(); // Store total pages for each genre
            
            foreach (var genre in genres)
            {
                var genreQuery = _context.Livros
                    .Include(l => l.IdAutorNavigation)
                    .Include(l => l.IdGenerosNavigation)
                    .Include(l => l.IdEditoraNavigation)
                    .Where(l => l.IdGeneros == genre.IdGeneros);
                    
                var totalBooksInGenre = genreQuery.Count();
                genrePages[genre.IdGeneros] = (int)Math.Ceiling(totalBooksInGenre / (double)PageSize);
                
                var books = genreQuery
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();
                    
                genreBooks[genre.IdGeneros] = books;
            }

            ViewBag.Generos = genres;
            ViewBag.GenreBooks = genreBooks;
            ViewBag.GenrePages = genrePages; // Pass total pages for each genre
            ViewBag.FeaturedCurrentPage = page;
            ViewBag.FeaturedTotalPages = featuredTotalPages;
            ViewBag.CurrentSection = section;
            
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                if (genreId.HasValue)
                {
                    ViewBag.CurrentPage = page;
                    ViewBag.TotalPages = genrePages[genreId.Value];
                    ViewBag.CurrentGenreId = genreId;
                    return PartialView("_BookCarousel", genreBooks[genreId.Value]);
                }
                else
                {
                    ViewBag.CurrentPage = page;
                    ViewBag.TotalPages = featuredTotalPages;
                    return PartialView("_BookCarousel", featuredBooks);
                }
            }
            
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
