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

        public RegisterGoogleModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterGoogleModel> logger,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _dbContext = dbContext;
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
            [Display(Name = "C�digo Postal")]
            [RegularExpression(@"^\d{4}-\d{3}$", ErrorMessage = "O C�digo Postal deve seguir o formato '1234-567'")]
            public string CodigoPostal { get; set; }

            [Required(ErrorMessage = "O n�mero de telem�vel � obrigat�rio.")]
            [Phone(ErrorMessage = "Por favor, insira um n�mero de telem�vel v�lido.")]
            [Display(Name = "N�mero de Telem�vel")]
            [RegularExpression(@"^\d{9}$", ErrorMessage = "O n�mero de telem�vel deve conter exatamente 9 d�itos num�ricos.")]
            public string PhoneNumber { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "Data de Nascimento")]
            public DateTime Birthday { get; set; }

            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; } // Este campo ser� preenchido automaticamente
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // Pega o e-mail do provedor externo
            var externalLogin = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLogin != null)
            {
                Input = new InputModel
                {
                    Email = externalLogin.Principal.FindFirstValue(ClaimTypes.Email) // Preenche com o e-mail do login externo
                };
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("/Home/Index");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // Obtemos o login externo
                var externalLogin = await _signInManager.GetExternalLoginInfoAsync();

                // Tenta encontrar um usu�rio existente com o e-mail fornecido
                var user = await _userManager.FindByEmailAsync(Input.Email);

                // Se o usu�rio n�o existir, cria um novo usu�rio
                if (user == null)
                {
                    user = new IdentityUser
                    {
                        UserName = Input.Email,
                        Email = Input.Email
                    };

                    // Cria o novo usu�rio no banco de dados
                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        // Se falhar a cria��o, exibe erros
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return Page();
                    }
                }

                // Se o login externo n�o foi associado ao usu�rio, associamos agora
                var resultAddLogin = await _userManager.AddLoginAsync(user, externalLogin);
                if (!resultAddLogin.Succeeded)
                {
                    // Se n�o conseguir associar o login externo, exibe erros
                    foreach (var error in resultAddLogin.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }

                // Agora adicionamos os dados adicionais ao perfil do usu�rio
                var codigoPostal = await _dbContext.CodigoPostals.FirstOrDefaultAsync(cp => cp.EndCodPostal == Input.CodigoPostal);
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

                var perfil = new Perfil
                {
                    EndMorada = $"{Input.Morada}, {Input.Cidade}",
                    EndCodPostal = Input.CodigoPostal,
                    NomePerfil = Input.Nome,
                    DataNascimentoPerfil = DateOnly.FromDateTime(Input.Birthday),
                    Apelido = Input.Apelido,
                    AspNetUserId = user.Id
                };

                _dbContext.Perfils.Add(perfil);
                await _dbContext.SaveChangesAsync();

                // Realiza o login do usu�rio ap�s a cria��o ou associa��o
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation("Usu�rio registrado e logado com sucesso.");

                // Redireciona para a p�gina inicial ou a URL fornecida
                return LocalRedirect(returnUrl);
            }

            return Page();
        }


        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("A UI padr�o exige um reposit�rio de usu�rios com suporte a e-mail.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
