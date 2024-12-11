using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Notauthorized()
        {
            return View();
        }
    }
}