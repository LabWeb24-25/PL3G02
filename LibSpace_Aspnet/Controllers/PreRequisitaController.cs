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
    public class PreRequisitaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PreRequisitaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PreRequisita
        public async Task<IActionResult> Index()
        {
            return View(await _context.PreRequisita.ToListAsync());
        }

        // GET: PreRequisita/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preRequisitum = await _context.PreRequisita
                .FirstOrDefaultAsync(m => m.Idreserva == id);
            if (preRequisitum == null)
            {
                return NotFound();
            }

            return View(preRequisitum);
        }

        // GET: PreRequisita/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PreRequisita/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idreserva,Idleitor,Idlivro,EstadoLevantamento")] PreRequisitum preRequisitum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(preRequisitum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(preRequisitum);
        }

        // GET: PreRequisita/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preRequisitum = await _context.PreRequisita.FindAsync(id);
            if (preRequisitum == null)
            {
                return NotFound();
            }
            return View(preRequisitum);
        }

        // POST: PreRequisita/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idreserva,Idleitor,Idlivro,EstadoLevantamento")] PreRequisitum preRequisitum)
        {
            if (id != preRequisitum.Idreserva)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(preRequisitum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PreRequisitumExists(preRequisitum.Idreserva))
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
            return View(preRequisitum);
        }

        // GET: PreRequisita/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preRequisitum = await _context.PreRequisita
                .FirstOrDefaultAsync(m => m.Idreserva == id);
            if (preRequisitum == null)
            {
                return NotFound();
            }

            return View(preRequisitum);
        }

        // POST: PreRequisita/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var preRequisitum = await _context.PreRequisita.FindAsync(id);
            if (preRequisitum != null)
            {
                _context.PreRequisita.Remove(preRequisitum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PreRequisitumExists(int id)
        {
            return _context.PreRequisita.Any(e => e.Idreserva == id);
        }
    }
}
