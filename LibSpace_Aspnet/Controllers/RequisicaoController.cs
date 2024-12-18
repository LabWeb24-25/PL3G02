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
            var Requisita = await _context.Requisita
                .Select(req => new RequisitaViewModel
                {
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
    }
}

