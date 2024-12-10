using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace LibSpace_Aspnet.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginWith2faModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginWith2faModel> _logger;

        public LoginWith2faModel(SignInManager<IdentityUser> signInManager, ILogger<LoginWith2faModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public bool RememberMe { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(7, ErrorMessage = "O {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Código de autenticação")]
            public string TwoFactorCode { get; set; }

            [Display(Name = "Lembrar este dispositivo")]
            public bool RememberMachine { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
        {
            // Certifica-te de que o utilizador já iniciou sessão antes de chegar aqui.
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                _logger.LogWarning("Não foi possível carregar o utilizador para autenticação de dois fatores.");
                throw new InvalidOperationException("Não foi possível carregar o utilizador.");
            }

            ReturnUrl = returnUrl;
            RememberMe = rememberMe;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                _logger.LogWarning("Não foi possível carregar o utilizador para autenticação de dois fatores.");
                throw new InvalidOperationException("Não foi possível carregar o utilizador.");
            }

            var authenticatorCode = Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, Input.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("Utilizador autenticado com 2FA.");
                return LocalRedirect(returnUrl ?? Url.Content("~/"));
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("Conta bloqueada devido a falhas consecutivas.");
                return RedirectToPage("./Lockout");
            }
            else
            {
                _logger.LogWarning("Código de autenticação inválido.");
                ModelState.AddModelError(string.Empty, "Código de autenticação inválido.");
                return Page();
            }
        }
    }
}
