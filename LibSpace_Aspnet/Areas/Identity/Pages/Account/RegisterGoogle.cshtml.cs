using LibSpace_Aspnet.Data;
using LibSpace_Aspnet.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibSpace_Aspnet.Areas.Identity.Pages.Account
{
    public class RegisterGoogleModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterGoogleModel> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RegisterGoogleModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterGoogleModel> logger,
            ApplicationDbContext dbContext,
             RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _dbContext = dbContext;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Nome")]
            public string Nome { get; set; }

            [Required]
            [Display(Name = "Apelido")]
            public string Apelido { get; set; }

            [Required]
            [Display(Name = "Morada")]
            public string Morada { get; set; }

            [Required]
            [Display(Name = "Cidade")]
            public string Cidade { get; set; }

            [Required]
            [Display(Name = "Código Postal")]
            [RegularExpression(@"^\d{4}-\d{3}$", ErrorMessage = "O Código Postal deve seguir o formato '1234-567'")]
            public string CodigoPostal { get; set; }

            [Required(ErrorMessage = "O número de telemóvel é obrigatório.")]
            [Phone(ErrorMessage = "Por favor, insira um número de telemóvel válido.")]
            [Display(Name = "Número de Telemóvel")]
            [RegularExpression(@"^\d{9}$", ErrorMessage = "O número de telemóvel deve conter exatamente 9 dígitos numéricos.")]
            public string PhoneNumber { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "Data de Nascimento")]
            public DateTime Birthday { get; set; }

            [EmailAddress]
            [Display(Name = "Email")]


            public string Email { get; set; } // Este campo será preenchido automaticamente

            [Required]
            [Display(Name = "Role")]
            public string Role { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null, string email = null)
        {
            // Obter todas as roles
            var roles = await _roleManager.Roles.ToListAsync();

            // Filtrar a função "Administrador"
            var filteredRoles = roles.Where(r => r.Name != "Admin").ToList();

            // Passar as roles filtradas para a ViewBag
            ViewData["Roles"] = filteredRoles;
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // Initialize Input if it's null
            Input ??= new InputModel();

            // Set email from parameter or from external login
            if (!string.IsNullOrEmpty(email))
            {
                Input.Email = email;
            }
            else
            {
                var externalLogin = await _signInManager.GetExternalLoginInfoAsync();
                if (externalLogin != null)
                {
                    Input.Email = externalLogin.Principal.FindFirstValue(ClaimTypes.Email);
                }
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            
            if (ModelState.IsValid)
            {
                try 
                {
                    // Get the user by email since we already created it in ExternalLogin
                    var user = await _userManager.FindByEmailAsync(Input.Email);
                    if (user == null)
                    {
                        _logger.LogError($"User not found for email {Input.Email}");
                        ModelState.AddModelError(string.Empty, "User not found.");
                        return Page();
                    }

                    // Update phone number
                    await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);


                    //Mudar aqui
                    if (Input.Role == "Bibliotecario")
                    {
                        // Se o papel for "bibliotecario", não atribui imediatamente
                        // Adiciona o utilizador à tabela de bibliotecários pendentes
                        var bibliotecarioPendente = new BibliotecarioPendente
                        {
                            AspNetUserId = user.Id,
                            ApplicationDate = DateTime.Now,
                            IsApproved = false
                        };
                        _dbContext.BibliotecarioPendentes.Add(bibliotecarioPendente);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        // Atribui o papel selecionado normalmente
                        await _userManager.AddToRoleAsync(user, Input.Role);
                    }



                    // Create or get CodigoPostal
                    var codigoPostal = await _dbContext.CodigoPostals
                        .FirstOrDefaultAsync(cp => cp.EndCodPostal == Input.CodigoPostal);
                        
                    if (codigoPostal == null)
                    {
                        codigoPostal = new CodigoPostal
                        {
                            EndCodPostal = Input.CodigoPostal,
                            EndLocalidade = Input.Cidade
                        };
                        _dbContext.CodigoPostals.Add(codigoPostal);
                        await _dbContext.SaveChangesAsync();
                    }

                    // Check if profile already exists
                    var existingPerfil = await _dbContext.Perfils
                        .FirstOrDefaultAsync(p => p.AspNetUserId == user.Id);

                    if (existingPerfil != null)
                    {
                        _logger.LogWarning($"Profile already exists for user {user.Id}");
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect("~/");
                    }

                    // Create Perfil
                    var perfil = new Perfil
                    {
                        EndMorada = $"{Input.Morada}, {Input.Cidade}",
                        EndCodPostal = Input.CodigoPostal,
                        NomePerfil = Input.Nome,
                        DataNascimentoPerfil = DateOnly.FromDateTime(Input.Birthday),
                        Apelido = Input.Apelido,
                        AspNetUserId = user.Id,
                        ImgPerfil = "/images/profiles/deafult_lab.webp" // Caminho da imagem padrão
                    };

                    _dbContext.Perfils.Add(perfil);
                    await _dbContext.SaveChangesAsync();

                    // Sign in the user
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation($"User {user.Email} profile created and signed in successfully");
                    
                    // Redirect to home page
                    return LocalRedirect("~/");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error creating profile: {ex}");
                    ModelState.AddModelError(string.Empty, "An error occurred while creating your profile.");
                    return Page();
                }
            }

            // Log model state errors
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _logger.LogError($"Model validation error: {error.ErrorMessage}");
                }
            }

            ViewData["Roles"] = _roleManager.Roles.ToList();
            return Page();
        }


        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("A UI padrão exige um repositório de usuários com suporte a e-mail.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
