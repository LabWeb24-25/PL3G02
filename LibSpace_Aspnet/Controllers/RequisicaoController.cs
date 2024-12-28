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
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Mensagem"] = "Por favor, faça login para aceder a esta página.";
                return Redirect("/Identity/Account/Login");
            }
            if (!User.IsInRole("Bibliotecario"))
            {
                return Redirect("/Users/Notauthorized");
            }
            var Requisita = await _context.Requisita
                .Select(req => new RequisitaViewModel
                {
                    IdLivro = req.IdLivro,
                    IdLeitor = req.IdLeitor,
                    IdBiblioRecetor = req.IdBibliotecarioRecetor,
                    IdBiblioRemetente = req.IdBibliotecarioRemetente,
                    NomeLivro = _context.Livros
                        .Where(l => l.IdLivro == req.IdLivro)
                        .Select(l => l.TituloLivros)
                        .FirstOrDefault(),
                    NomeLeitor = _context.Perfils
                        .Where(lec => lec.IdPerfil == req.IdLeitor)
                        .Select(lec => lec.NomePerfil)
                        .FirstOrDefault(),
                    DataRequisicao = req.DataRequisicao,
                    DataPrevEntrega = req.DataPrevEntrega,
                    DataEntrega = req.DataEntrega,
                    NomeBibliotecarioRecetor = _context.Perfils
                        .Where(b => b.IdPerfil == req.IdBibliotecarioRecetor)
                        .Select(b => b.NomePerfil)
                        .FirstOrDefault(),
                    NomeBibliotecarioRemetente = req.IdBibliotecarioRemetente.HasValue
                        ? _context.Perfils
                            .Where(b => b.IdPerfil == req.IdBibliotecarioRemetente.Value)
                            .Select(b => b.NomePerfil)
                            .FirstOrDefault()
                        : "N/A"  // Se nulo, exibir "N/A"
                })
                .ToListAsync();


            var PreRequisita = _context.PreRequisita
                    .Select(pre => new PreRequisitaViewModel
                    {
                        IdReserva = pre.Idreserva,
                        NomeLivro = _context.Livros
                            .Where(l => l.IdLivro == pre.Idlivro)
                            .Select(l => l.TituloLivros)
                            .FirstOrDefault(),
                        NomeLeitor = _context.Perfils
                            .Where(leitor => leitor.IdPerfil == pre.Idleitor)
                            .Select(leitor => leitor.NomePerfil)
                            .FirstOrDefault(),
                        EstadoLevantamento = pre.EstadoLevantamento
                    })
                    .ToList();



            var viewModel = new RequisitaView
            {
                Requisita = Requisita,
                PreRequisita = PreRequisita
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> AceitarReq(int idprereq)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Utilizador não autenticado." });
            }
            if (!User.IsInRole("Bibliotecario"))
            {
                return Json(new { success = false, message = "Fuunção exclusiva a Bibliotecários" });
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


            var prerequisita = await _context.PreRequisita.FindAsync(idprereq);
            if (prerequisita == null)
            {
                return Json(new { success = false, message = "Erro" });
            }

            prerequisita.EstadoLevantamento = 1;

            var requisita = new Requisitum
            {
                IdLeitor = prerequisita.Idleitor,
                IdBibliotecarioRecetor = perfil.IdPerfil,
                IdLivro = prerequisita.Idlivro,
                DataRequisicao = DateTime.Now,
                DataPrevEntrega = DateOnly.FromDateTime(DateTime.Now.AddDays(15)),
            };

            
            try
            {
                _context.Requisita.Add(requisita);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Livro Requisitado" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Erro ao Requisitar: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RejeitarReq(int idprereq)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Utilizador não autenticado." });
            }
            if (!User.IsInRole("Bibliotecario"))
            {
                return Json(new { success = false, message = "Fuunção exclusiva a Bibliotecários" });
            }

            var prerequisita = await _context.PreRequisita.FindAsync(idprereq);
            if (prerequisita == null)
            {
                return Json(new { success = false, message = "Erro" });
            }

            var livro = await _context.Livros.FindAsync(prerequisita.Idlivro);
            livro.NumExemplares = livro.NumExemplares + 1;
            prerequisita.EstadoLevantamento = -1;

            return Json(new { success = true, message = "Livro Requisitado" });
            

        }
        [HttpPost]
        public async Task<IActionResult> FinalizarReq([FromBody] RequisitaViewModel requisita)
        {
            // Validação dos dados recebidos
            if (requisita == null || string.IsNullOrWhiteSpace(requisita.NomeLivro))
            {
                return Json(new { success = false, message = "Dados inválidos ou incompletos." });
            }

            try
            {
                // Busca o registro no banco de dados com base no nome do livro
                var _requisita = await _context.Requisita
                    .Include(r => r.IdLivroNavigation) // Certifica-se de incluir a navegação para o título do livro
                    .FirstOrDefaultAsync(r => r.IdLivro == requisita.IdLivro && r.IdBibliotecarioRecetor==requisita.IdBiblioRecetor && r.IdBibliotecarioRemetente == requisita.IdBiblioRemetente && r.DataRequisicao==r.DataRequisicao);

                
                if (_requisita == null)
                {
                    return Json(new { success = false, message = "Requisição não encontrada para o livro especificado." });
                }

                // Realize as alterações necessárias no registro (_requisita)
                // Exemplo: Atualizar a data de entrega ou estado da requisição
                _requisita.DataEntrega = DateTime.Now;
                _context.Requisita.Update(_requisita);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Requisição finalizada com sucesso." });
            }
            catch (Exception ex)
            {
                // Log da exceção (opcional, mas recomendado para depuração)
                // _logger.LogError(ex, "Erro ao finalizar requisição.");

                return Json(new { success = false, message = "Ocorreu um erro ao finalizar a requisição. Por favor, tente novamente mais tarde." });
            }
        }

    }
}

