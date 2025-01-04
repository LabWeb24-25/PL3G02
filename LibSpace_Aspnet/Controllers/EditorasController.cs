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
using Microsoft.AspNetCore.Http; // Adicionei para usar IFormFile
using LibSpace.Models; // Este using pode não ser necessário, verifique

namespace LibSpace_Aspnet.Controllers
{
    [ServiceFilter(typeof(PermitFilter))]
    public class EditorasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PermitFilter _permitFilter;

        public EditorasController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, PermitFilter permitFilter)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _permitFilter = permitFilter;
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
        public async Task<IActionResult> Livro_Create(Editora editora, IFormFile? ImgEditora)
        {
            if (ModelState.IsValid)
            {
                if (ImgEditora != null)
                {
                    var Extensoes = new[] { ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(ImgEditora.FileName).ToLower();

                    if (!Extensoes.Contains(extension))
                    {
                        ModelState.AddModelError("ImgEditora", "Introduza uma imagem válida");
                        return View(editora);
                    }
                    editora.ImgEditora = Path.GetFileName(ImgEditora.FileName);

                    string coverFileName = Path.GetFileName(ImgEditora.FileName);
                    string coverFullPath = Path.Combine(_webHostEnvironment.WebRootPath, "Editora_IMG", coverFileName);
                    using (var stream = new FileStream(coverFullPath, FileMode.Create))
                    {
                        await ImgEditora.CopyToAsync(stream);
                    }

                }
                else
                {
                    editora.ImgEditora = "editorasemfoto.png";
                }

                _context.Add(editora);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Livroes", new { idEditora = editora.IdEditora });
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
        public async Task<IActionResult> Create(Editora editora, IFormFile? ImgEditora)
        {

            if (ModelState.IsValid)
            {

                if (ImgEditora != null)
                {
                    var Extensoes = new[] { ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(ImgEditora.FileName).ToLower();

                    if (!Extensoes.Contains(extension))
                    {
                        ModelState.AddModelError("ImgEditora", "Introduza uma imagem válida");
                        return View(editora);
                    }
                    editora.ImgEditora = Path.GetFileName(ImgEditora.FileName);

                    string coverFileName = Path.GetFileName(ImgEditora.FileName);
                    string coverFullPath = Path.Combine(_webHostEnvironment.WebRootPath, "Editora_IMG", coverFileName);
                    using (var stream = new FileStream(coverFullPath, FileMode.Create))
                    {
                        await ImgEditora.CopyToAsync(stream);
                    }

                }
                else
                {
                    editora.ImgEditora = "editorasemfoto.png";
                }
                _context.Add(editora);
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

            ViewBag.IdEditora = editora.IdEditora;
            ViewBag.ImgEditora = "~/Editora_IMG/" + editora.ImgEditora;

            return View(editora);
        }


        // POST: Editoras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Editora editora, IFormFile? ImgEditora)
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
                        if (!string.IsNullOrEmpty(editoraToUpdate.ImgEditora) && editoraToUpdate.ImgEditora != "editorasemfoto.png")
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

            ViewBag.ImgEditora = "~/Editora_IMG/" + editora.ImgEditora;
            return View(editora);
        }

        [HttpPost]
        public IActionResult Delete(int idEditora)
        {
            // Buscar editora pelo ID
            var editora = _context.Editoras.FirstOrDefault(e => e.IdEditora == idEditora);

            if (editora == null)
            {
                TempData["ErrorMessage"] = "Editora não encontrada.";
                return RedirectToAction("Index");
            }

            // Procurar todos os livros onde tenham a editora como referência
            var livrosDaEditora = _context.Livros.Where(l => l.IdEditora == idEditora).ToList();
            if (livrosDaEditora.Any())
            {
                // Verificar se algum livro da editora tem requisições pendentes
                var requisicoesPendentes = _context.Requisita
                    .Where(r => livrosDaEditora.Select(l => l.IdLivro).Contains(r.IdLivro) && r.DataEntrega == null)
                    .ToList();

                if (requisicoesPendentes.Any())
                {
                    TempData["ErrorMessage"] = "Existem requisições não finalizadas para livros desta editora!";
                    return RedirectToAction("Details", new { id = idEditora });
                }

                // Verificar se existem pré-requisições pendentes para os livros da editora
                var prerequisicoes = _context.PreRequisita
                    .Where(r => livrosDaEditora.Select(l => l.IdLivro).Contains(r.Idlivro))
                    .ToList();

                if (prerequisicoes.Any())
                {
                    _context.PreRequisita.RemoveRange(prerequisicoes);
                }

                // Remover todos os livros da editora
                _context.Livros.RemoveRange(livrosDaEditora);
                _context.SaveChanges();
            }

            try
            {
                // Remover a editora
                _context.Editoras.Remove(editora);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Editora e livros associados excluídos com sucesso.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao excluir a editora e livros associados: {ex.Message}";
            }

            // Redirecionar para a página anterior (o usuário estava nela quando iniciou a exclusão)
            return Redirect(Request.Headers["Referer"].ToString());
        }





        private bool EditoraExists(int id)
        {
            return _context.Editoras.Any(e => e.IdEditora == id);
        }
    }
}