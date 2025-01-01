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
using LibSpace.Models;

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
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)] // A cache dura 1 minuto

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
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Mensagem"] = "Por favor, faça login para aceder a esta página.";
                return Redirect("/Identity/Account/Login");
            }
            if (!User.IsInRole("Bibliotecario"))
            {   
                return Redirect("/Acess/Notauthorized");
            }
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
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Mensagem"] = "Por favor, faça login para aceder a esta página.";
                return Redirect("/Identity/Account/Login");
            }
            if (!User.IsInRole("Bibliotecario"))
            {
                return Redirect("/Acess/Notauthorized");
            }
            return View();
        }

        // POST: Editoras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NomeEditora,InfoEditora,ImgEditora")] EditoraViewModel editora)
        {

            if (ModelState.IsValid)
            {
                var _Editora = new Editora();
                _Editora.NomeEditora = editora.NomeEditora;
                _Editora.InfoEditora = editora.InfoEditora;
                if (editora.ImgEditora != null)
                {
                    var Extensoes = new[] { ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(editora.ImgEditora.FileName).ToLower();
                    _Editora.ImgEditora = Path.GetFileName(editora.ImgEditora.FileName);


                    string coverFileName = Path.GetFileName(editora.ImgEditora.FileName);
                    string coverFullPath = Path.Combine(_webHostEnvironment.WebRootPath, "Editora_IMG", coverFileName);
                    using (var stream = new FileStream(coverFullPath, FileMode.Create))
                    {
                        await editora.ImgEditora.CopyToAsync(stream);
                    }
                }
                else
                {
                    _Editora.ImgEditora = "editorasemfoto.png";
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
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Mensagem"] = "Por favor, faça login para aceder a esta página.";
                return Redirect("/Identity/Account/Login");
            }
            if (!User.IsInRole("Bibliotecario"))
            {
                return Redirect("/Acess/Notauthorized");
            }

            var editora = await _context.Editoras.FindAsync(id);
            if (editora == null)
            {
                return NotFound();
            }
            var editoraviewmodel = new EditoraViewModel
            {
                NomeEditora = editora.NomeEditora,
                InfoEditora = editora.InfoEditora
            };
            ViewBag.IdEditora = editora.IdEditora;
            ViewBag.ImgEditora = "~/Editora_IMG/" + editora.ImgEditora;
            return View(editoraviewmodel);
        }


        // POST: Editoras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEditora,NomeEditora,InfoEditora,ImgEditora")] Editora editora, IFormFile? ImgEditora)
        {
            if (id != editora.IdEditora)
            {
                return NotFound();
            }

            var editoraToUpdate = await _context.Editoras.FindAsync(id);
            if (editoraToUpdate == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Atualiza os campos de texto
                    editoraToUpdate.NomeEditora = editora.NomeEditora;
                    editoraToUpdate.InfoEditora = editora.InfoEditora;

                    // Verifica se um novo arquivo de imagem foi enviado
                    if (ImgEditora != null)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                        var extension = Path.GetExtension(ImgEditora.FileName).ToLower();

                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError(nameof(ImgEditora), "Formato inválido. Apenas JPG, JPEG ou PNG são permitidos.");
                            return View(editora);
                        }

                        // Apaga a imagem antiga SE EXISTIR
                        if (!string.IsNullOrEmpty(editoraToUpdate.ImgEditora))
                        {
                            string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Editora_IMG", editoraToUpdate.ImgEditora);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Salva a nova imagem
                        string newFileName = Guid.NewGuid() + extension;
                        string newFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Editora_IMG", newFileName);
                        using (var stream = new FileStream(newFilePath, FileMode.Create))
                        {
                            await ImgEditora.CopyToAsync(stream);
                        }

                        // Atualiza o caminho da nova imagem
                        editoraToUpdate.ImgEditora = newFileName;
                    }
                    // Se ImgEditora for null, não faz nada, mantendo a imagem antiga.

                    // Salva as alterações
                    _context.Update(editoraToUpdate);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
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
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Mensagem"] = "Por favor, faça login para aceder a esta página.";
                return Redirect("/Identity/Account/Login");
            }
            if (!User.IsInRole("Bibliotecario"))
            {
                return Redirect("/Users/Notauthorized");
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
