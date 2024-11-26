using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibSpace_Aspnet.Data;
using LibSpace_Aspnet.Models;

namespace LibSpace_Aspnet.Controllers
{
    public class LivroesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LivroesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Livroes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Livros.Include(l => l.IdAutorNavigation).Include(l => l.IdEditoraNavigation).Include(l => l.IdGenerosNavigation).Include(l => l.IdLinguaNavigation);
            return View(await applicationDbContext.ToListAsync());
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

            return View(livro);
        }

        // GET: Livroes/Create
        public IActionResult Create()
        {
            ViewData["IdAutor"] = new SelectList(_context.Autors, "IdAutor", "IdAutor");
            ViewData["IdEditora"] = new SelectList(_context.Editoras, "IdEditora", "IdEditora");
            ViewData["IdGeneros"] = new SelectList(_context.Generos, "IdGeneros", "IdGeneros");
            ViewData["IdLingua"] = new SelectList(_context.Pais, "IdPais", "IdPais");
            return View();
        }

        // POST: Livroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLivro,Isbn,DataEdicao,TituloLivros,NumExemplares,CapaImg,IdEditora,IdLingua,Sinopse,IdAutor,IdGeneros")] Livro livro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(livro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAutor"] = new SelectList(_context.Autors, "IdAutor", "IdAutor", livro.IdAutor);
            ViewData["IdEditora"] = new SelectList(_context.Editoras, "IdEditora", "IdEditora", livro.IdEditora);
            ViewData["IdGeneros"] = new SelectList(_context.Generos, "IdGeneros", "IdGeneros", livro.IdGeneros);
            ViewData["IdLingua"] = new SelectList(_context.Pais, "IdPais", "IdPais", livro.IdLingua);
            return View(livro);
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
