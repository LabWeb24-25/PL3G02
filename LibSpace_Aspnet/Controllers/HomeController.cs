using LibSpace_Aspnet.Data;
using LibSpace_Aspnet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using LibSpace_Aspnet.Data;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Security.Claims;

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


        public async Task<IActionResult> Index(int page = 1)
        {
            var info = await _context.Bibliotecas.FirstOrDefaultAsync();
            if (info == null && (User.IsInRole("Admin") || User.IsInRole("Bibliotecario")))
            {
                return RedirectToAction("FirstStep", "Home");
            }
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

            ViewBag.requisicoesemfalta = null;
            if (User.Identity.IsAuthenticated && User.IsInRole("Leitor"))
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                {
                    return Json(new { success = false, message = "Não foi possível identificar o usuário." });
                }
                // Converte o Guid para string para facilitar a manipulação e visualização
                var userId = userIdClaim.Value;

                var perfil = await _context.Perfils
                    .FirstOrDefaultAsync(p => p.AspNetUserId == userId);
                var requisicoes = _context.Requisita
                    .Where(l => l.IdLeitor == perfil.IdPerfil && l.DataPrevEntrega <= DateOnly.FromDateTime(DateTime.Now))
                    .Select(r => new RequisicaoEmAtrasoViewModel
                    {
                        NomeLivro = r.IdLivroNavigation.TituloLivros, // Substitua por sua propriedade real de IdLivro
                        NomeLeitor = perfil.NomePerfil, // Substitua por sua propriedade real de NomeLeitor
                        DataPrevEntrega = r.DataPrevEntrega
                    }).ToList();
                ViewBag.requisicoesemfalta = requisicoes;
            }
            if (User.Identity.IsAuthenticated && User.IsInRole("Bibliotecario"))
            {
                var requisicoesAtrasadas = (from r in _context.Requisita
                                            join p in _context.Perfils on r.IdLeitor equals p.IdPerfil
                                            where r.DataPrevEntrega <= DateOnly.FromDateTime(DateTime.Now)
                                            select new RequisicaoEmAtrasoViewModel
                                            {
                                                NomeLivro = r.IdLivroNavigation.TituloLivros, // Nome do livro
                                                NomeLeitor = p.NomePerfil, //Nome do Perfil a ir buscar na tabela Perfil
                                                DataPrevEntrega = r.DataPrevEntrega
                                            }).ToList();
                ViewBag.requisicoesemfalta = requisicoesAtrasadas;
            }
            return View(featuredBooks);
        }

        public async Task<IActionResult> Sobre()
        {
            var info = await _context.Bibliotecas.FirstOrDefaultAsync();
            if (info == null && (User.IsInRole("Admin") || User.IsInRole("Bibliotecario")))
            {
                return RedirectToAction("FirstStep", "Home");
            }
            if(info== null && !User.IsInRole("Admin") && !User.IsInRole("Bibliotecario"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View(info);
        }

        public IActionResult FirstStep()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  
        public async Task<IActionResult> FirstStep([Bind("IdBiblioteca,NomeBiblioteca, Email, Telefone, Horario, EndMorada, EndCodPostal, EndLocalidade")] Biblioteca biblioteca)
        {
            if (ModelState.IsValid)
            {
                _context.Add(biblioteca);
                await _context.SaveChangesAsync();
                return RedirectToAction("Sobre", "Home", new { idBiblioteca = biblioteca.IdBiblioteca });
            }
            return View(biblioteca);
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
