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
    public class RequisicaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequisicaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Requisicao
        public async Task<IActionResult> Index()
        {
            var requisitas = await _context.Requisita.ToListAsync();

            var prerequisitas = await _context.PreRequisita.ToListAsync();

            var viewModel = new RequisitaViewModel
            {
                Requisita = requisitas,
                PreRequisita = prerequisitas
            };

            return View(viewModel);
        }

        // GET: Requisicao/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requisitum = await _context.Requisita
                .Include(r => r.IdLivroNavigation)
                .FirstOrDefaultAsync(m => m.IdLeitor == id);
            if (requisitum == null)
            {
                return NotFound();
            }

            return View(requisitum);
        }

        // GET: Requisicao/Create
        public IActionResult Create()
        {
            ViewData["IdLivro"] = new SelectList(_context.Livros, "IdLivro", "IdLivro");
            return View();
        }

        // POST: Requisicao/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLeitor,IdBibliotecarioRecetor,IdBibliotecarioRemetente,IdLivro,DataRequisicao,DataPrevEntrega,DataEntrega")] Requisitum requisitum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(requisitum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdLivro"] = new SelectList(_context.Livros, "IdLivro", "IdLivro", requisitum.IdLivro);
            return View(requisitum);
        }

        // GET: Requisicao/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requisitum = await _context.Requisita.FindAsync(id);
            if (requisitum == null)
            {
                return NotFound();
            }
            ViewData["IdLivro"] = new SelectList(_context.Livros, "IdLivro", "IdLivro", requisitum.IdLivro);
            return View(requisitum);
        }

        // POST: Requisicao/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdLeitor,IdBibliotecarioRecetor,IdBibliotecarioRemetente,IdLivro,DataRequisicao,DataPrevEntrega,DataEntrega")] Requisitum requisitum)
        {
            if (id != requisitum.IdLeitor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requisitum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequisitumExists(requisitum.IdLeitor))
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
            ViewData["IdLivro"] = new SelectList(_context.Livros, "IdLivro", "IdLivro", requisitum.IdLivro);
            return View(requisitum);
        }

        // GET: Requisicao/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requisitum = await _context.Requisita
                .Include(r => r.IdLivroNavigation)
                .FirstOrDefaultAsync(m => m.IdLeitor == id);
            if (requisitum == null)
            {
                return NotFound();
            }

            return View(requisitum);
        }

        // POST: Requisicao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requisitum = await _context.Requisita.FindAsync(id);
            if (requisitum != null)
            {
                _context.Requisita.Remove(requisitum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequisitumExists(int id)
        {
            return _context.Requisita.Any(e => e.IdLeitor == id);
        }
    }
}
