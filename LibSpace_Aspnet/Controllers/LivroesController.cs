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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        public async Task<IActionResult> Index(string filter, string query, int? authorId, int? editorId, int? countryId)
        {
            var livrosQuery = _context.Livros.AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                livrosQuery = livrosQuery.Where(l => EF.Functions.Like(l.TituloLivros.ToLower(), $"%{query.ToLower()}%"));
            }

            if (filter == "clicks")
            {
                livrosQuery = livrosQuery.OrderByDescending(l => l.Clicks);
            }
            else if (authorId.HasValue)
            {
                livrosQuery = livrosQuery.Where(l => l.IdAutor == authorId);
            }
            else if (editorId.HasValue)
            {
                livrosQuery = livrosQuery.Where(l => l.IdEditora == editorId);
            }
            else if (countryId.HasValue)
            {
                livrosQuery = livrosQuery.Where(l => l.IdLinguaNavigation.IdPais == countryId);
            }

            var livros = await livrosQuery
                .Include(l => l.IdAutorNavigation)
                .Include(l => l.IdEditoraNavigation)
                .Include(l => l.IdGenerosNavigation)
                .Include(l => l.IdLinguaNavigation)
                .ToListAsync();

            ViewData["Query"] = query;
            ViewBag.Autores = await _context.Autors.ToListAsync();
            ViewBag.Editoras = await _context.Editoras.ToListAsync();
            ViewBag.Paises = await _context.Pais.ToListAsync();

            return View(livros);
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

        [HttpPost]
        public async Task<IActionResult> CreateAutor(string nomeAutor, string? pseudonimo, DateOnly dataNascimento, string bibliografia)
        {
            if (string.IsNullOrWhiteSpace(nomeAutor))
            {
                return BadRequest(new { errors = new { NomeAutor = "O nome é obrigatório.", Pseudonimo = "O pseudônimo é obrigatório." } });
            }

            var autor = new Autor
            {
                NomeAutor = nomeAutor,
                Pseudonimo = pseudonimo,
                DataNascimento = dataNascimento,
                Bibliografia = bibliografia,
                IdLingua = 1,
                FotoAutor = "autorsemfoto.jpeg"
            };

            _context.Add(autor);
            await _context.SaveChangesAsync();

            return Json(new { id = autor.IdAutor, nome = autor.NomeAutor });
        }

        public async Task<IActionResult> CreateEditora(string nomeEditora, string infoEditora)
        {
            if (string.IsNullOrWhiteSpace(nomeEditora))
            {
                return BadRequest(new { errors = new { NomeEditora = "O nome é obrigatório." } });
            }

            var editora = new Editora
            {
                NomeEditora = nomeEditora,
                InfoEditora = infoEditora,
                ImgEditora = "editorasemfoto.png"
            };

            _context.Add(editora);
            await _context.SaveChangesAsync();

            return Json(new { id = editora.IdEditora, nome = editora.NomeEditora });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateGenero([FromBody] string nomeGenero)
        {
            if (string.IsNullOrWhiteSpace(nomeGenero) || nomeGenero.Length > 20)
            {
                return Json(new { errors = "Nome do Gênero é obrigatório e deve ter no máximo 20 caracteres." });
            }

            var genero = new Genero { NomeGeneros = nomeGenero };

            _context.Generos.Add(genero);
            _context.SaveChanges(); // Salva as alterações no banco

            // Retorna o gênero recém-criado
            return Json(new { id = genero.IdGeneros, nome = genero.NomeGeneros });
        }




        public async Task<IActionResult> Requisitar(int? id)
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

            if (!User.IsInRole("Leitor"))
            {
                return Redirect("/Users/Notauthorized");
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
            if (livro == null || livro.NumExemplares ==0)
            {
                return NotFound();
            }

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Json(new { success = false, message = "Não foi possível identificar o usuário." });
            }


            // Converte o Guid para string para facilitar a manipulação e visualização
            var userId = userIdClaim.Value;

            var perfil = await _context.Perfils
                .FirstOrDefaultAsync(p => p.AspNetUserId == userId);
            livro.NumExemplares = livro.NumExemplares - 1;
            var requisicao = new PreRequisitum
            {
                Idlivro = livro.IdLivro,
                Idleitor = perfil.IdPerfil,
                EstadoLevantamento = 0
            };

            _context.Add(requisicao);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = livro.IdLivro });
        }



        // GET: Livroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Verifica se o ID é nulo
            if (id == null)
            {
                return NotFound();
            }
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            ViewBag.estreq=0;
           

            // Declaração inicial do ViewBag para evitar redundâncias
            ViewBag.BibliotecarioId = null;
            ViewBag.IsFavorito = false;
            
            // Verifica se o usuário está autenticado e é um administrador
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                // Busca a associação do livro com o bibliotecário
                var livroAssociacao = await _context.InserirLivros
                    .Where(f => f.IdLivro == id)
                    .Select(f => new { f.IdLivro, f.IdBibliotecario })
                    .FirstOrDefaultAsync();

                var perfilB = await _context.Perfils
                .FirstOrDefaultAsync(p => p.IdPerfil == livroAssociacao.IdBibliotecario);

                ViewBag.BibliotecarioId = perfilB.NomePerfil;
            }

            // Verifica se o usuário está autenticado e é um leitor
            if (User.Identity.IsAuthenticated && User.IsInRole("Leitor"))
            {
                // Obtém o ID do usuário atual
                var userId = userIdClaim.Value;

                var perfil = await _context.Perfils
                    .FirstOrDefaultAsync(p => p.AspNetUserId == userId);

                    // Verifica se o livro está nos favoritos do usuário
                var isFavorito = await _context.Favoritos
                        .AnyAsync(f => f.IdLeitor == userId && f.IdLivro == id);
                ViewBag.IsFavorito = isFavorito;
                

                var preRequisitos = await _context.PreRequisita
                .Where(pr => pr.Idlivro == id && pr.Idleitor == perfil.IdPerfil) // Supondo que PreRequisita tenha um campo LivroId
                .ToListAsync();

                var Requisitos = await _context.Requisita
                    .Where(r => r.IdLivro == id && r.IdLeitor == perfil.IdPerfil && r.DataEntrega == null) // Supondo que PreRequisita tenha um campo LivroId
                    .ToListAsync();
                if (preRequisitos.Any())
                {
                    ViewBag.estreq = 1;
                }
                if (Requisitos.Any())
                {
                    ViewBag.estreq = 2;
                }
            }

            // Busca os detalhes do livro e suas associações
            var livro = await _context.Livros
                .Include(l => l.IdAutorNavigation)
                .Include(l => l.IdEditoraNavigation)
                .Include(l => l.IdGenerosNavigation)
                .Include(l => l.IdLinguaNavigation)
                .FirstOrDefaultAsync(m => m.IdLivro == id);

            // Verifica se o livro foi encontrado
            if (livro == null)
            {
                return NotFound();
            }

            // Incrementa os cliques no livro e salva a alteração no banco de dados
            livro.Clicks += 1;
            _context.Livros.Update(livro);
            await _context.SaveChangesAsync();

            // Retorna a view com os detalhes do livro
            return View(livro);
        }
 
        [HttpPost]
        public async Task<IActionResult> AdicionarFavorito(int idLivro)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Usuário não autenticado." });
            }

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Json(new { success = false, message = "Não foi possível identificar o usuário." });
            }

            var userId = userIdClaim.Value;

            var livro = await _context.Livros.FindAsync(idLivro);
            if (livro == null)
            {
                return Json(new { success = false, message = "Livro não encontrado." });
            }

            var favorito = new Favorito
            {
                IdLivro = idLivro,
                IdLeitor = userId, // Now using AspNetUserId directly
            };

            try
            {
                _context.Favoritos.Add(favorito);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Livro adicionado aos favoritos com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Erro ao adicionar aos favoritos: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoverFavorito(int idLivro)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Usuário não autenticado." });
            }

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Json(new { success = false, message = "Não foi possível identificar o usuário." });
            }

            var userId = userIdClaim.Value;

            var livro = await _context.Livros.FindAsync(idLivro);
            if (livro == null)
            {
                return Json(new { success = false, message = "Livro não encontrado." });
            }

            var favoritoExistente = await _context.Favoritos
                .FirstOrDefaultAsync(f => f.IdLivro == idLivro && f.IdLeitor == userId);

            if (favoritoExistente == null)
            {
                return Json(new { success = false, message = "Livro não está nos favoritos." });
            }

            _context.Favoritos.Remove(favoritoExistente);

            try
            {
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Livro removido dos favoritos com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Erro ao remover dos favoritos: {ex.Message}" });
            }
        }

        // GET: Livroes/Create

        public IActionResult Create(int? idEditora, int? idAutor, int? idPais, int? idGenero)
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    TempData["Mensagem"] = "Por favor, faça login para aceder a esta página.";
            //    return Redirect("/Identity/Account/Login");
            //}
            //if (!User.IsInRole("Bibliotecario"))
            //{
            //    return Redirect("/Users/Notauthorized");
            //}
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
                string coverFileName;

                if (livroViewModel.CapaImg != null && livroViewModel.CapaImg.Length > 0)
                {
                    // Validate the cover image format
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                    var fileExtension = Path.GetExtension(livroViewModel.CapaImg.FileName).ToLower();

                    // Save the image to a folder
                    coverFileName = Path.GetFileName(livroViewModel.CapaImg.FileName);
                    var coverPath = Path.Combine(_webHostEnvironment.WebRootPath, "Cover", coverFileName);

                    using (var stream = new FileStream(coverPath, FileMode.Create))
                    {
                        await livroViewModel.CapaImg.CopyToAsync(stream);
                    }
                }
                else
                {
                    // Define a imagem padrão caso nenhuma seja enviada
                    coverFileName = "livrosemfoto.png";
                }

                // Create a Livro object and populate its fields
                var livro = new Livro
                {
                    Clicks = 0,
                    Isbn = livroViewModel.Isbn,
                    DataEdicao = livroViewModel.DataEdicao, // Convert DateTime to DateOnly
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
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                {
                    return Json(new { success = false, message = "Não foi possível identificar o utilizador." });
                }

                var userId = userIdClaim.Value;

                var perfilB = await _context.Perfils
                    .FirstOrDefaultAsync(p => p.AspNetUserId == userId);

                //var inserirlivro = new InserirLivro
                //{
                //    IdLivro = livro.IdLivro,
                //    IdBibliotecario = perfilB.IdPerfil
                //};

                //_context.Add(inserirlivro);
                //await _context.SaveChangesAsync();
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

            if (!User.Identity.IsAuthenticated)
            {
                TempData["Mensagem"] = "Por favor, faça login para aceder a esta página.";
                return Redirect("/Identity/Account/Login");
            }
            if (!User.IsInRole("Bibliotecario"))
            {
                return Redirect("/Users/Notauthorized");
            }

            var livro = await _context.Livros.FindAsync(id);
            if (livro == null)
            {
                return NotFound();
            }
            var _livro = new LivroViewModel
            {
                Isbn = livro.Isbn,
                TituloLivros = livro.TituloLivros,
                DataEdicao = livro.DataEdicao,
                NumExemplares = livro.NumExemplares,
                Sinopse = livro.Sinopse,
               
            };
  
            ViewData["IdAutor"] = new SelectList(_context.Autors, "IdAutor", "NomeAutor", livro.IdAutor);
            ViewData["IdEditora"] = new SelectList(_context.Editoras, "IdEditora", "NomeEditora", livro.IdEditora);
            ViewData["IdGeneros"] = new SelectList(_context.Generos, "IdGeneros", "NomeGeneros", livro.IdGeneros);
            ViewData["IdLingua"] = new SelectList(_context.Pais, "IdPais", "NomePais", livro.IdLingua);
            ViewBag.CapaImg = "~/Cover/" + livro.CapaImg;
            return View(_livro);
        }

        // POST: Livroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Isbn,DataEdicao,TituloLivros,NumExemplares,CapaImg,IdEditora,IdLingua,Sinopse,IdAutor,IdGeneros,statusimg")] LivroViewModel livro)
        {
            if (ModelState.IsValid)
            {
                var _livro = await _context.Livros.FindAsync(id);
                if (livro.CapaImg == null)
                {
                    if (livro.statusimg == 1 && _livro.CapaImg != "livrosemfoto.png")
                    {
                        var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Cover", _livro.CapaImg);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                    }
                    _livro.CapaImg = "livrosemfoto.png"; // ou definir como o valor padrão
                }
                else
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                    var fileExtension = Path.GetExtension(livro.CapaImg.FileName).ToLower();

                    // Save the image to a folder
                    var coverFileName = Path.GetFileName(livro.CapaImg.FileName);
                    _livro.CapaImg = coverFileName;
                    var coverPath = Path.Combine(_webHostEnvironment.WebRootPath, "Cover", coverFileName);

                    using (var stream = new FileStream(coverPath, FileMode.Create))
                    {
                        await livro.CapaImg.CopyToAsync(stream);
                    }
                }
                _livro.Isbn = livro.Isbn;
                _livro.DataEdicao = livro.DataEdicao;
                _livro.TituloLivros = livro.TituloLivros;
                _livro.NumExemplares = livro.NumExemplares;
                _livro.IdEditora = livro.IdEditora;
                _livro.IdLingua = livro.IdLingua;
                _livro.Sinopse = livro.Sinopse;
                _livro.IdAutor = livro.IdAutor;
                _livro.IdGeneros = livro.IdGeneros;

                try
                {
                    _context.Update(_livro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LivroExists(_livro.IdAutor))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = _livro.IdLivro });
            }
            ViewData["IdAutor"] = new SelectList(_context.Autors, "IdAutor", "NomeAutor", livro.IdAutor);
            ViewData["IdEditora"] = new SelectList(_context.Editoras, "IdEditora", "NomeEditora", livro.IdEditora);
            ViewData["IdGeneros"] = new SelectList(_context.Generos, "IdGeneros", "NomeGeneros", livro.IdGeneros);
            ViewData["IdLingua"] = new SelectList(_context.Pais, "IdPais", "NomePais", livro.IdLingua);
            return View(livro);
        }

        // GET: Livroes/Delete/5
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
