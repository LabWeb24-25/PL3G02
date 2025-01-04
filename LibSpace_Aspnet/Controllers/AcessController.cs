using Microsoft.AspNetCore.Mvc;

namespace LibSpace_Aspnet.Controllers
{
    public class AcessController : Controller
    {
        [ResponseCache(Duration = 100000, Location = ResponseCacheLocation.Client)] // A cache dura 100000 segundos
        public IActionResult Notauthorized()
        {
            return View();
        }
    }
}
