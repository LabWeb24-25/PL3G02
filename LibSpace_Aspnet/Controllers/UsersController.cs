using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibSpace_Aspnet.Data;
using LibSpace_Aspnet.Models;
using System.Security.Claims;

namespace LibSpace_Aspnet.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Perfils.Include(p => p.EndCodPostalNavigation);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfil = await _context.Perfils
                .Include(p => p.EndCodPostalNavigation)
                .FirstOrDefaultAsync(m => m.IdPerfil == id);
            if (perfil == null)
            {
                return NotFound();
            }

            return View(perfil);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["EndCodPostal"] = new SelectList(_context.CodigoPostals, "EndCodPostal", "EndCodPostal");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPerfil,EndMorada,EndCodPostal,NomePerfil,DataNascimentoPerfil,Apelido,ImgPerfil,AspNetUserId")] Perfil perfil)
        {
            if (ModelState.IsValid)
            {
                _context.Add(perfil);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EndCodPostal"] = new SelectList(_context.CodigoPostals, "EndCodPostal", "EndCodPostal", perfil.EndCodPostal);
            return View(perfil);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfil = await _context.Perfils.FindAsync(id);
            if (perfil == null)
            {
                return NotFound();
            }
            ViewData["EndCodPostal"] = new SelectList(_context.CodigoPostals, "EndCodPostal", "EndCodPostal", perfil.EndCodPostal);
            return View(perfil);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPerfil,EndMorada,EndCodPostal,NomePerfil,DataNascimentoPerfil,Apelido,ImgPerfil,AspNetUserId")] Perfil perfil)
        {
            if (id != perfil.IdPerfil)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(perfil);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerfilExists(perfil.IdPerfil))
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
            ViewData["EndCodPostal"] = new SelectList(_context.CodigoPostals, "EndCodPostal", "EndCodPostal", perfil.EndCodPostal);
            return View(perfil);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfil = await _context.Perfils
                .Include(p => p.EndCodPostalNavigation)
                .FirstOrDefaultAsync(m => m.IdPerfil == id);
            if (perfil == null)
            {
                return NotFound();
            }

            return View(perfil);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var perfil = await _context.Perfils.FindAsync(id);
            if (perfil != null)
            {
                _context.Perfils.Remove(perfil);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PerfilExists(int id)
        {
            return _context.Perfils.Any(e => e.IdPerfil == id);
        }

        public async Task<IActionResult> Perfil()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var perfil = await _context.Perfils
                .FirstOrDefaultAsync(p => p.AspNetUserId == userId);

            if (perfil == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Details), new { id = perfil.IdPerfil });
        }
    }
}
