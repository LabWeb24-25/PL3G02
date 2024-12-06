using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibSpace_Aspnet.Data;
using LibSpace_Aspnet.Models;
using LibSpace.Models;

namespace LibSpace_Aspnet.Controllers
{
    public class LivroesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LivroesController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _webHostEnvironment = environment;
        }

        // GET: Livroes
        public async Task<IActionResult> Index(string query)
        {
            // Comece com a consulta básica de Livros
            var livrosQuery = _context.Livros.AsQueryable();

            // Verifique se a query foi passada e se não é vazia
            if (!string.IsNullOrEmpty(query))
            {
                // Tornar ambos os lados da comparação minúsculos para uma busca insensível a maiúsculas/minúsculas
                livrosQuery = livrosQuery.Where(l => EF.Functions.Like(l.TituloLivros.ToLower(), $"%{query.ToLower()}%"));
            }

            // Inclua as entidades relacionadas (as navegações) após aplicar o filtro
            livrosQuery = livrosQuery
                          .Include(l => l.IdAutorNavigation)
                          .Include(l => l.IdEditoraNavigation)
                          .Include(l => l.IdGenerosNavigation)
                          .Include(l => l.IdLinguaNavigation);

            // Execute a consulta e obtenha os resultados
            var livros = await livrosQuery.ToListAsync();

            return View(livros);
        }






        public async Task<IActionResult> Requisitar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros
                .Include(l => l.IdAutorNavigation)
                .Include(l => l.IdEditoraNavigation)
                .Include(l => l.IdGenerosNavigation)
                .Include(l => l.IdLinguaNavigation)
                .FirstOrDefaultAsync(m => m.IdLivro == id);
            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Requisitar(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var livro = await _context.Livros.FindAsync(id);
            if (livro == null)
            {
                return NotFound();
            }

            var requisicao = new PreRequisitum
            {
                Idlivro = livro.IdLivro,
                Idleitor = 1, // Ajuste isso para capturar o leitor correto.
                EstadoLevantamento = 0
            };

            _context.Add(requisicao);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = livro.IdLivro });
        }



        // GET: Livroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros
                .Include(l => l.IdAutorNavigation)
                .Include(l => l.IdEditoraNavigation)
                .Include(l => l.IdGenerosNavigation)
                .Include(l => l.IdLinguaNavigation)
                .FirstOrDefaultAsync(m => m.IdLivro == id);
            if (livro == null)
            {
                return NotFound();
            }
            livro.Clicks += 1;
            _context.Livros.Update(livro); // Marca o objeto como modificado.
            await _context.SaveChangesAsync(); // Salva as mudanças no banco de dados.

            return View(livro);
        }


        // GET: Livroes/Create
        public IActionResult Create(int? idEditora, int? idAutor, int? idPais, int? idGenero)
        {
            ViewData["IdAutor"] = new SelectList(_context.Autors, "IdAutor", "NomeAutor", idAutor);
            ViewData["IdEditora"] = new SelectList(_context.Editoras, "IdEditora", "NomeEditora", idEditora); // Preenche com a editora selecionada
            ViewData["IdGeneros"] = new SelectList(_context.Generos, "IdGeneros", "NomeGeneros", idGenero);
            ViewData["IdLingua"] = new SelectList(_context.Pais, "IdPais", "NomePais", idPais);

            return View();
        }


        // POST: Livroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Isbn,DataEdicao,TituloLivros,NumExemplares,CapaImg,IdEditora,IdLingua,Sinopse,IdAutor,IdGeneros")] LivroViewModel livroViewModel)
        {
            if (ModelState.IsValid)
            {
                // Validate the cover image format
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                var fileExtension = Path.GetExtension(livroViewModel.CapaImg?.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("CapaImg", "Please upload a valid image file (jpg, jpeg, png, gif, bmp).");
                    PopulateMockDropdowns();
                    return View(livroViewModel);
                }

                // Save the image to a folder
                var coverFileName = Path.GetFileName(livroViewModel.CapaImg.FileName);
                var coverPath = Path.Combine(_webHostEnvironment.WebRootPath, "Cover", coverFileName);

                using (var stream = new FileStream(coverPath, FileMode.Create))
                {
                    await livroViewModel.CapaImg.CopyToAsync(stream);
                }

                // Create a Livro object and populate its fields
                var livro = new Livro
                {
                    Clicks = 0,
                    Isbn = livroViewModel.Isbn,
                    DataEdicao = DateOnly.FromDateTime(livroViewModel.DataEdicao), // Convert DateTime to DateOnly
                    TituloLivros = livroViewModel.TituloLivros,
                    NumExemplares = livroViewModel.NumExemplares,
                    IdEditora = livroViewModel.IdEditora,
                    IdLingua = livroViewModel.IdLingua,
                    Sinopse = livroViewModel.Sinopse,
                    IdAutor = livroViewModel.IdAutor,
                    IdGeneros = livroViewModel.IdGeneros,
                    CapaImg = coverFileName // Save the file name in the database
                };


                // Simulate database save
                _context.Add(livro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            // Repopulate dropdowns for the view in case of an error
            PopulateMockDropdowns();
            return View(livroViewModel);
        }

        // Mock data for dropdowns
        private void PopulateMockDropdowns()
        {
            // Mock data for dropdowns
                    ViewData["IdAutor"] = new SelectList(new List<object>
            {
                new { IdAutor = 1, Name = "Autor 1" },
                new { IdAutor = 2, Name = "Autor 2" }
            }, "IdAutor", "Name");

                    ViewData["IdEditora"] = new SelectList(new List<object>
            {
                new { IdEditora = 1, Name = "Editora 1" },
                new { IdEditora = 2, Name = "Editora 2" }
            }, "IdEditora", "Name");

                    ViewData["IdGeneros"] = new SelectList(new List<object>
            {
                new { IdGeneros = 1, Name = "Genero 1" },
                new { IdGeneros = 2, Name = "Genero 2" }
            }, "IdGeneros", "Name");

                    ViewData["IdLingua"] = new SelectList(new List<object>
            {
                new { IdLingua = 1, Name = "Língua 1" },
                new { IdLingua = 2, Name = "Língua 2" }
            }, "IdLingua", "Name");
        }

        // GET: Livroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros.FindAsync(id);
            if (livro == null)
            {
                return NotFound();
            }
            ViewData["IdAutor"] = new SelectList(_context.Autors, "IdAutor", "IdAutor", livro.IdAutor);
            ViewData["IdEditora"] = new SelectList(_context.Editoras, "IdEditora", "IdEditora", livro.IdEditora);
            ViewData["IdGeneros"] = new SelectList(_context.Generos, "IdGeneros", "IdGeneros", livro.IdGeneros);
            ViewData["IdLingua"] = new SelectList(_context.Pais, "IdPais", "IdPais", livro.IdLingua);
            return View(livro);
        }

        // POST: Livroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdLivro,Isbn,DataEdicao,TituloLivros,NumExemplares,CapaImg,IdEditora,IdLingua,Sinopse,IdAutor,IdGeneros")] Livro livro)
        {
            if (id != livro.IdLivro)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(livro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LivroExists(livro.IdLivro))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAutor"] = new SelectList(_context.Autors, "IdAutor", "IdAutor", livro.IdAutor);
            ViewData["IdEditora"] = new SelectList(_context.Editoras, "IdEditora", "IdEditora", livro.IdEditora);
            ViewData["IdGeneros"] = new SelectList(_context.Generos, "IdGeneros", "IdGeneros", livro.IdGeneros);
            ViewData["IdLingua"] = new SelectList(_context.Pais, "IdPais", "IdPais", livro.IdLingua);
            return View(livro);
        }

        // GET: Livroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros
                .Include(l => l.IdAutorNavigation)
                .Include(l => l.IdEditoraNavigation)
                .Include(l => l.IdGenerosNavigation)
                .Include(l => l.IdLinguaNavigation)
                .FirstOrDefaultAsync(m => m.IdLivro == id);
            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        // POST: Livroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var livro = await _context.Livros.FindAsync(id);
            if (livro != null)
            {
                _context.Livros.Remove(livro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LivroExists(int id)
        {
            return _context.Livros.Any(e => e.IdLivro == id);
        }
    }
}
