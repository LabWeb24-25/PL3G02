using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibSpace_Aspnet.Data;
using LibSpace_Aspnet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace YourNamespace.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public UsersController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;


        }

        public async Task<IActionResult> Index()
        {
            var pendingBibliotecarios = await _context.BibliotecarioPendentes
                .Include(b => b.AspNetUser)
                .Where(b => !b.IsApproved)
                .ToListAsync();

            var allUsers = await _userManager.Users.ToListAsync();

            var userRoles = new Dictionary<string, List<string>>();
            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles[user.Id] = roles.ToList();
            }

            var viewModel = new UsersViewModel
            {
                PendingBibliotecarios = pendingBibliotecarios,
                AllUsers = allUsers,
                UserRoles = userRoles
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PromoteUsertoAdmin(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "ID do utilizador inválido.";
                return RedirectToAction(nameof(Index));
            }


            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                TempData["Error"] = "ID do utilizador inválido.";
                return RedirectToAction(nameof(Index));
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    TempData["Error"] = "Utilizador não encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                await _userManager.RemoveFromRoleAsync(user, "Leitor");
                await _userManager.AddToRoleAsync(user, "Admin");

                await _context.SaveChangesAsync();

                var userEmail = await _userManager.GetEmailAsync(user);

                await _emailSender.SendEmailAsync(
                    userEmail,
                    "Utilizador Promovido",
                    "Você foi promovido para administrador.");
                                

                TempData["Success"] = "Utilizador promovido com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Erro ao processar o pedido.";
                return RedirectToAction(nameof(Index));
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveBibliotecario(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "ID do utilizador inválido.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var pendingBibliotecario = await _context.BibliotecarioPendentes
                    .FirstOrDefaultAsync(b => b.AspNetUserId == userId && !b.IsApproved);

                if (pendingBibliotecario == null)
                {
                    TempData["Error"] = "Pedido de bibliotecário não encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    TempData["Error"] = "Utilizador não encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                // Remove Leitor role and add Bibliotecario role
                await _userManager.RemoveFromRoleAsync(user, "Leitor");
                await _userManager.AddToRoleAsync(user, "Bibliotecario");

                // Remove from pending table
                _context.BibliotecarioPendentes.Remove(pendingBibliotecario);
                await _context.SaveChangesAsync();

                var userEmail = await _userManager.GetEmailAsync(user);

                await _emailSender.SendEmailAsync(
                    userEmail,
                    "Pedido Aceite",
                    "Você foi aceite como bibliotecário.");
                                

                TempData["Success"] = "Bibliotecário aprovado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Erro ao processar o pedido.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectBibliotecario(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "ID do utilizador inválido.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var pendingBibliotecario = await _context.BibliotecarioPendentes
                    .FirstOrDefaultAsync(b => b.AspNetUserId == userId && !b.IsApproved);

                if (pendingBibliotecario == null)
                {
                    TempData["Error"] = "Pedido de bibliotecário não encontrado.";
                    return RedirectToAction(nameof(Index));
                }


                // Just remove from pending table
                _context.BibliotecarioPendentes.Remove(pendingBibliotecario);
                await _context.SaveChangesAsync();

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    TempData["Error"] = "Utilizador não encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                var userEmail = await _userManager.GetEmailAsync(user);

                await _emailSender.SendEmailAsync(
                    userEmail,
                    "Pedido Rejeitado",
                    "Você não foi aceite como bibliotecário.");



                TempData["Success"] = "Pedido rejeitado com sucesso.";

                //Apagar o perfil
                // Apagar o perfil associado ao userId

                var perfil = await _context.Perfils.SingleOrDefaultAsync(p => p.AspNetUserId == userId);
                if (perfil != null)
                {
                    _context.Perfils.Remove(perfil);
                    await _context.SaveChangesAsync();
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Ocorreu um erro inesperado ao apagar o utilizador.");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Erro ao processar o pedido.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserDetails(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID cannot be null or empty.");
            }

            try 
            {
                // Debug logging
                Console.WriteLine($"Fetching details for userId: {userId}");
                
                var userProfile = await _context.Perfils
                    .Include(p => p.EndCodPostalNavigation)
                    .FirstOrDefaultAsync(p => p.AspNetUserId == userId);

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                // Get ALL roles for the user and log them
                var roles = await _userManager.GetRolesAsync(user);
                Console.WriteLine($"User roles found: {string.Join(", ", roles)}");

                var viewModel = new UserDetailsViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber ?? "Não disponível",
                    Nome = userProfile?.NomePerfil ?? user.UserName,
                    Apelido = userProfile?.Apelido ?? "Não disponível",
                    Morada = userProfile?.EndMorada ?? "Não disponível",
                    CodigoPostal = userProfile?.EndCodPostal ?? "Não disponível",
                    Localidade = userProfile?.EndCodPostalNavigation?.EndLocalidade ?? "Não disponível",
                    DataNascimento = userProfile?.DataNascimentoPerfil.ToString("dd/MM/yyyy") ?? "Não disponível",
                    // Show all roles if user has multiple
                    Role = roles.Any() ? string.Join(", ", roles) : "Sem papel atribuído",
                    ImgPerfil = userProfile?.ImgPerfil
                };

                return PartialView("_UserDetailsPartial", viewModel);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching user details for ID {userId}:");
                Console.Error.WriteLine($"Exception type: {ex.GetType().Name}");
                Console.Error.WriteLine($"Message: {ex.Message}");
                Console.Error.WriteLine($"Stack trace: {ex.StackTrace}");
                
                return StatusCode(500, "Erro ao carregar detalhes do utilizador");
            }
        }



        public class UserActionModel
        {
            public string UserId { get; set; }
        }
    }
}