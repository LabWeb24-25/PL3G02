using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibSpace_Aspnet.Data;
using LibSpace_Aspnet.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace LibSpace_Aspnet.Controllers
{
    [Authorize(Roles = "Leitor")]
    public class LivrosRequisitadosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LivrosRequisitadosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var perfil = await _context.Perfils
                .FirstOrDefaultAsync(p => p.AspNetUserId == userId);

            if (perfil == null)
            {
                return NotFound();
            }

            var preRequisitados = await (from pr in _context.PreRequisita
                                       where pr.Idleitor == perfil.IdPerfil && pr.EstadoLevantamento == 0
                                       select pr.Idlivro).ToListAsync();

            var livros = await _context.Livros
                               .Where(l => preRequisitados.Contains(l.IdLivro))
                               .ToListAsync();

            var requisitados = await (from r in _context.Requisita
                                    join l in _context.Livros on r.IdLivro equals l.IdLivro
                                    join p in _context.Perfils on r.IdBibliotecarioRecetor equals p.IdPerfil
                                    join u in _context.Users on p.AspNetUserId equals u.Id
                                    where r.IdLeitor == perfil.IdPerfil && r.DataEntrega == null
                                    select new RequisitadoInfo
                                    {
                                        Livro = l,
                                        DataRequisicao = r.DataRequisicao,
                                        DataPrevEntrega = r.DataPrevEntrega,
                                        BibliotecarioNome = u.UserName
                                    }).ToListAsync();

            var entregues = await (from r in _context.Requisita
                      join l in _context.Livros on r.IdLivro equals l.IdLivro
                      join p1 in _context.Perfils on r.IdBibliotecarioRecetor equals p1.IdPerfil
                      join p2 in _context.Perfils on r.IdBibliotecarioRemetente equals p2.IdPerfil
                      join u1 in _context.Users on p1.AspNetUserId equals u1.Id
                      join u2 in _context.Users on p2.AspNetUserId equals u2.Id
                      where r.IdLeitor == perfil.IdPerfil && r.DataEntrega != null
                      select new EntregueInfo
                      {
                          Livro = l,
                          DataRequisicao = r.DataRequisicao,
                          DataPrevEntrega = r.DataPrevEntrega,
                          DataEntrega = r.DataEntrega.Value,
                          BibliotecarioNome = u1.UserName,
                          BibliotecarioRemetenteNome = u2.UserName
                      }).ToListAsync();

            var viewModel = new LivrosRequisitadosViewModel
            {
                PreRequisitados = livros,
                Requisitados = requisitados,
                Entregues = entregues
            };

            return View(viewModel);
        }
    }
} 