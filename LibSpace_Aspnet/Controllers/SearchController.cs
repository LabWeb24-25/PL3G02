using LibSpace_Aspnet.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class SearchController : Controller
{
    private readonly ApplicationDbContext _context;

    public SearchController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [HttpGet]
    public JsonResult GetBooks(string query)
    {
        var books = _context.Livros
            .Where(b => b.TituloLivros.Contains(query))
            .Select(b => new
            {
                b.IdLivro,
                b.TituloLivros,
                b.IdAutorNavigation.NomeAutor,
                b.CapaImg
            })
            .ToList();

        return Json(books);
    }


}

