using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibSpace_Aspnet.Data;
using LibSpace_Aspnet.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace LibSpace_Aspnet.Controllers
{
    public class EditorasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EditorasController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Editoras
        public async Task<IActionResult> Index()
        {
            return View(await _context.Editoras.ToListAsync());
        }

        // GET: Editoras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editora = await _context.Editoras
                .FirstOrDefaultAsync(m => m.IdEditora == id);
            if (editora == null)
            {
                return NotFound();
            }

            return View(editora);
        }

        public IActionResult Livro_Create()
        {
            return View();
        }

        // POST: Editoras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Livro_Create([Bind("NomeEditora,InfoEditora,ImgEditora")] EditoraViewModel editora)
        {
            var Extensoes = new[] { ".jpg", ".jpeg", ".png" };

            var extension = Path.GetExtension(editora.ImgEditora.FileName).ToLower();

            if (!Extensoes.Contains(extension))
            {
                ModelState.AddModelError("ImgEditora", "Introduza uma imagem válida");
            }
            if (ModelState.IsValid)
            {
                var _Editora = new Editora();
                _Editora.NomeEditora = editora.NomeEditora;
                _Editora.InfoEditora = editora.InfoEditora;
                _Editora.ImgEditora = Path.GetFileName(editora.ImgEditora.FileName);


                string coverFileName = Path.GetFileName(editora.ImgEditora.FileName);
                string coverFullPath = Path.Combine(_webHostEnvironment.WebRootPath, "Editora_IMG", coverFileName);
                using (var stream = new FileStream(coverFullPath, FileMode.Create))
                {
                    await editora.ImgEditora.CopyToAsync(stream);
                }
                _context.Add(_Editora);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Livroes", new { idEditora = _Editora.IdEditora });
            }
            return View(editora);
        }


        // GET: Editoras/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Editoras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NomeEditora,InfoEditora,ImgEditora")] EditoraViewModel editora)
        {
            var Extensoes = new[] { ".jpg", ".jpeg", ".png" };

            var extension = Path.GetExtension(editora.ImgEditora.FileName).ToLower();

            if (!Extensoes.Contains(extension))
            {
                ModelState.AddModelError("ImgEditora", "Introduza uma imagem válida");
            }
            if (ModelState.IsValid)
            {
                var _Editora = new Editora();
                _Editora.NomeEditora= editora.NomeEditora;
                _Editora.InfoEditora= editora.InfoEditora;
                _Editora.ImgEditora = Path.GetFileName(editora.ImgEditora.FileName);


                string coverFileName=Path.GetFileName(editora.ImgEditora.FileName);
                string coverFullPath = Path.Combine(_webHostEnvironment.WebRootPath, "Editora_IMG", coverFileName);
                using (var stream = new FileStream(coverFullPath, FileMode.Create))
                {
                    await editora.ImgEditora.CopyToAsync(stream);
                }
                _context.Add(_Editora);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(editora);
        }

        // GET: Editoras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editora = await _context.Editoras.FindAsync(id);
            if (editora == null)
            {
                return NotFound();
            }
            return View(editora);
        }

        // POST: Editoras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEditora,NomeEditora,InfoEditora,ImgEditora")] Editora editora)
        {
            if (id != editora.IdEditora)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(editora);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EditoraExists(editora.IdEditora))
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
            return View(editora);
        }

        // GET: Editoras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editora = await _context.Editoras
                .FirstOrDefaultAsync(m => m.IdEditora == id);
            if (editora == null)
            {
                return NotFound();
            }

            return View(editora);
        }

        // POST: Editoras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var editora = await _context.Editoras.FindAsync(id);
            if (editora != null)
            {
                _context.Editoras.Remove(editora);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EditoraExists(int id)
        {
            return _context.Editoras.Any(e => e.IdEditora == id);
        }
    }
}
