using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibSpace_Aspnet.Data;
using LibSpace_Aspnet.Models;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;

namespace LibSpace_Aspnet.Controllers
{
    public class AutorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public AutorsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _webHostEnvironment = environment;

        }

        // GET: Autors
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Autors.Include(a => a.IdLinguaNavigation);
            return View(await applicationDbContext.ToListAsync());
        }

        //// GET: Autors/Details/5
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)] // A cache dura 1 minuto
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autors
                .Include(a => a.IdLinguaNavigation)
                .FirstOrDefaultAsync(m => m.IdAutor == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }


        [HttpPost]
        public async Task<IActionResult> Createpais([FromBody] Pai pais)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
            }

            _context.Pais.Add(pais);
            await _context.SaveChangesAsync(); // Use SaveChangesAsync para operações assíncronas
            var paises = await _context.Pais.ToListAsync(); // Obtenha a lista de países de forma assíncrona
            return Json(new { id = pais.IdPais, nome = pais.NomePais, paises });
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
            ViewData["IdLingua"] = new SelectList(_context.Pais, "IdPais", "NomePais");
            return View();
        }

        // POST: Autors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Livro_Create(AutorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string? uniqueFileName = null;

                if (viewModel.FotoAutor != null)
                {
                    // Validate the image format
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                    var fileExtension = Path.GetExtension(viewModel.FotoAutor.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("FotoAutor", "Por favor, carregue um arquivo de imagem válido (jpg, jpeg, png, gif, bmp).");

                        return View(viewModel);
                    }

                    // Generate a unique file name to prevent overwriting, poder ter que adicionar aqui um id unico
                    uniqueFileName = Path.GetFileName(viewModel.FotoAutor.FileName);
                    var autorImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Foto_Autor", uniqueFileName);

                    using (var fileStream = new FileStream(autorImagePath, FileMode.Create))
                    {
                        await viewModel.FotoAutor.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    uniqueFileName = "autorsemfoto.jpeg";
                }


                // Create Autor object and populate fields
                var autor = new Autor
                {
                    NomeAutor = viewModel.NomeAutor,
                    DataNascimento = viewModel.DataNascimento,
                    Pseudonimo = viewModel.Pseudonimo,
                    // Handle nullable DateTime for DataFalecimento
                    DataFalecimento = viewModel.DataFalecimento,
                    Bibliografia = viewModel.Bibliografia,
                    IdLingua = viewModel.IdLingua,
                    FotoAutor = uniqueFileName // Save the file name in the database
                };

                _context.Add(autor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Livroes", new { idAutor = autor.IdAutor });
            }


            return View(viewModel);
        }

        // GET: Autors/Create
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
            ViewData["IdLingua"] = new SelectList(_context.Pais, "IdPais", "NomePais");
            return View();
        }

        // POST: Autors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AutorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string? uniqueFileName = null;

                if (viewModel.FotoAutor == null)
                {
                    uniqueFileName = "autorsemfoto.jpeg";
                }
                else
                {
                    // Validate the image format
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                    var fileExtension = Path.GetExtension(viewModel.FotoAutor.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("FotoAutor", "Por favor, carregue um arquivo de imagem válido (jpg, jpeg, png, gif, bmp).");

                        return View(viewModel);
                    }

                    // Generate a unique file name to prevent overwriting, poder ter que adicionar aqui um id unico
                    uniqueFileName = Path.GetFileName(viewModel.FotoAutor.FileName);
                    var autorImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Foto_Autor", uniqueFileName);

                    using (var fileStream = new FileStream(autorImagePath, FileMode.Create))
                    {
                        await viewModel.FotoAutor.CopyToAsync(fileStream);
                    }
                }
                

                // Create Autor object and populate fields
                var autor = new Autor
                {
                    NomeAutor = viewModel.NomeAutor,
                    DataNascimento = viewModel.DataNascimento,
                    Pseudonimo = viewModel.Pseudonimo,
                    DataFalecimento = viewModel.DataFalecimento,
                    Bibliografia = viewModel.Bibliografia,
                    IdLingua = viewModel.IdLingua,
                    FotoAutor = uniqueFileName // Save the file name in the database
                };

                _context.Add(autor);
                await _context.SaveChangesAsync();
                var previousUrl = Request.Headers["Referer"].ToString();
                return RedirectToAction(nameof(Details), new { id = autor.IdAutor });
            }


            return View(viewModel);
        }



        // GET: Autors/Edit/5
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

            var autor = await _context.Autors.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }
            var _autor = new AutorViewModel
            {
                NomeAutor = autor.NomeAutor,
                Pseudonimo = autor.Pseudonimo,
                DataNascimento = autor.DataNascimento,
                DataFalecimento = autor.DataFalecimento,
                Bibliografia = autor.Bibliografia,
            };
            ViewBag.IdAutor = autor.IdAutor;
            ViewData["IdLingua"] = new SelectList(
                await _context.Pais.ToListAsync(),
                "IdPais",
                "NomePais",
                autor.IdLingua
            );
            ViewBag.FotoAutor = "~/Foto_Autor/" + autor.FotoAutor;
            return View(_autor);
        }

        // POST: Autors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Autors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAutor,NomeAutor,DataNascimento,Pseudonimo,DataFalecimento,Bibliografia,IdLingua")] Autor autor, IFormFile NewPhoto, string OldPhoto)
        {
            if (id != autor.IdAutor)
            {
                return NotFound();
            }

            // Validação de Data: Garantir que DataNascimento não seja a data padrão (obrigatória)
            if (autor.DataNascimento == default)
            {
                ModelState.AddModelError("DataNascimento", "A data de nascimento é obrigatória.");
            }

            // (DataFalecimento é opcional)

            if (ModelState.IsValid)
            {
                try
                {
                    // --- Prioridade para atualizar a foto ---
                    if (NewPhoto != null)
                    {
                        // Validar o formato da imagem
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                        var fileExtension = Path.GetExtension(NewPhoto.FileName).ToLower();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("NewPhoto", "Por favor, carregue um arquivo de imagem válido (jpg, jpeg, png, gif, bmp).");
                            // Restaurar a foto antiga no modelo para que seja exibida novamente
                            autor.FotoAutor = OldPhoto;
                            ViewData["IdLingua"] = new SelectList(_context.Pais, "IdPais", "NomePais", autor.IdLingua);
                            return View(autor);
                        }

                        // Gerar um nome único para o novo arquivo
                        string uniqueFileName = Path.GetFileName(NewPhoto.FileName);
                        var autorImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Foto_Autor", uniqueFileName);

                        // Salvar o novo arquivo
                        using (var fileStream = new FileStream(autorImagePath, FileMode.Create))
                        {
                            await NewPhoto.CopyToAsync(fileStream);
                        }

                        // Atualizar o nome da foto no objeto autor
                        autor.FotoAutor = uniqueFileName;

                        // Excluir a foto antiga, se existir
                        if (!string.IsNullOrEmpty(OldPhoto))
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Foto_Autor", OldPhoto);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                    }
                    else
                    {
                        // Se nenhuma nova foto for carregada, manter a foto antiga
                        autor.FotoAutor = OldPhoto;
                    }

                    // --- Fim da lógica de atualização da foto ---

                    // Atualizar os outros campos do autor (incluindo as datas)
                    _context.Update(autor); // Certifique-se de que esta linha está presente
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorExists(autor.IdAutor))
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

            // Se o ModelState não for válido, retornar para a view com os erros
            // Restaurar a foto antiga se houve erro na validação da nova foto
            autor.FotoAutor = OldPhoto;
            ViewData["IdLingua"] = new SelectList(_context.Pais, "IdPais", "NomePais", autor.IdLingua);
            return View(autor);
        }



        [HttpPost]
        public IActionResult Delete(int idAutor)
        {
            // Buscar autor pelo ID
            var autor = _context.Autors.FirstOrDefault(a => a.IdAutor == idAutor);

            if (autor == null)
            {
                TempData["ErrorMessage"] = "Autor não encontrado.";
                return RedirectToAction("Index");
            }

            // Procurar todos os livros onde tenham o autor como referência
            var livrosDoAutor = _context.Livros.Where(l => l.IdAutor == idAutor).ToList();
            if (livrosDoAutor.Any())
            {
                // Verificar se algum livro do autor tem requisições pendentes
                var requisicoesPendentes = _context.Requisita
                    .Where(r => livrosDoAutor.Select(l => l.IdLivro).Contains(r.IdLivro) && r.DataEntrega == null)
                    .ToList();

                if (requisicoesPendentes.Any())
                {
                    TempData["ErrorMessage"] = "Existem requisições não finalizadas para livros deste autor!";
                    return RedirectToAction("Details", new { id = idAutor });
                }
                var prerequisicoes = _context.PreRequisita
                .Where(r => livrosDoAutor.Select(l => l.IdLivro).Contains(r.Idlivro))
                    .ToList();

                if (prerequisicoes != null)
                {
                    _context.PreRequisita.RemoveRange(prerequisicoes);
                }
                // Remover todos os livros do autor
                _context.Livros.RemoveRange(livrosDoAutor);
                _context.SaveChanges();
            }

            try
            {
                // Remover o autor
                _context.Autors.Remove(autor);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Autor e livros associados excluídos com sucesso.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao excluir o autor e livros associados: {ex.Message}";
            }

            // Redirecionar para a página anterior (o usuário estava nela quando iniciou a exclusão)
            return Redirect(Request.Headers["Referer"].ToString());
        }


        private bool AutorExists(int id)
        {
            return _context.Autors.Any(e => e.IdAutor == id);
        }
    }
}
