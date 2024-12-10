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
    public class PaisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pais
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pais.ToListAsync());
        }

        // GET: Pais/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pai = await _context.Pais
                .FirstOrDefaultAsync(m => m.IdPais == id);
            if (pai == null)
            {
                return NotFound();
            }

            return View(pai);
        }

        // GET: Pais/Create
        public IActionResult Livro_Create()
        {
            return View();
        }

        // POST: Pais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Livro_Create([Bind("IdPais,NomePais")] Pai pai)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pai);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Livroes", new { idPais = pai.IdPais });
            }
            return View(pai);
        }

        // GET: Pais/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPais,NomePais")] Pai pai)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pai);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Livroes", new { idPais = pai.IdPais });
            }
            return View(pai);
        }

       
        // GET: Pais/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pai = await _context.Pais
                .FirstOrDefaultAsync(m => m.IdPais == id);
            if (pai == null)
            {
                return NotFound();
            }

            return View(pai);
        }

        // POST: Pais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pai = await _context.Pais.FindAsync(id);
            if (pai != null)
            {
                _context.Pais.Remove(pai);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaiExists(int id)
        {
            return _context.Pais.Any(e => e.IdPais == id);
        }
    }
}
