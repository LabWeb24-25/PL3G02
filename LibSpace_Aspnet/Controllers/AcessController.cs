using LibSpace_Aspnet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibSpace_Aspnet.Controllers
{
    public class AcessController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PermitFilter _permitFilter;
        public AcessController(ApplicationDbContext context, PermitFilter permitFilter)
        {
            _context = context;
            _permitFilter = permitFilter;
        }


        [ResponseCache(Duration = 100000, Location = ResponseCacheLocation.Client)] // A cache dura 100000 segundos
        public IActionResult LockOut()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Json(new { success = false, message = "Não foi possível identificar o usuário." });
            }
            var userId = userIdClaim.Value;
            var bloqueio = _context.Bloquears
                          .FirstOrDefault(b => b.IdUser ==userId && b.EstadoBloqueio); 
            return View(bloqueio);
        }

        [ResponseCache(Duration = 100000, Location = ResponseCacheLocation.Client)] // A cache dura 100000 segundos
        [ServiceFilter(typeof(PermitFilter))]
        public IActionResult Notauthorized()
        {
            return View();
        }
    }
}
