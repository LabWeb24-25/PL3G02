// Licenciado para a .NET Foundation ao abrigo de um ou mais acordos.
// A .NET Foundation licencia este ficheiro ao abrigo da licença MIT.
#nullable disable

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
    public class LoginWithRecoveryCodeModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<LoginWithRecoveryCodeModel> _logger;

        public LoginWithRecoveryCodeModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<LoginWithRecoveryCodeModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        ///     Esta API suporta a infraestrutura de UI padrão do ASP.NET Core Identity e não se destina a ser usada
        ///     diretamente a partir do seu código. Esta API pode mudar ou ser removida em versões futuras.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     Esta API suporta a infraestrutura de UI padrão do ASP.NET Core Identity e não se destina a ser usada
        ///     diretamente a partir do seu código. Esta API pode mudar ou ser removida em versões futuras.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     Esta API suporta a infraestrutura de UI padrão do ASP.NET Core Identity e não se destina a ser usada
        ///     diretamente a partir do seu código. Esta API pode mudar ou ser removida em versões futuras.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     Esta API suporta a infraestrutura de UI padrão do ASP.NET Core Identity e não se destina a ser usada
            ///     diretamente a partir do seu código. Esta API pode mudar ou ser removida em versões futuras.
            /// </summary>
            [BindProperty]
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Código de Recuperação")]
            public string RecoveryCode { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            // Garantir que o utilizador passou pela página de nome de utilizador e palavra-passe primeiro
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Não foi possível carregar o utilizador de autenticação de dois fatores.");
            }

            ReturnUrl = returnUrl;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Não foi possível carregar o utilizador de autenticação de dois fatores.");
            }

            var recoveryCode = Input.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            var userId = await _userManager.GetUserIdAsync(user);

            if (result.Succeeded)
            {
                _logger.LogInformation("O utilizador com o ID '{UserId}' iniciou sessão com um código de recuperação.", user.Id);
                return LocalRedirect(returnUrl ?? Url.Content("~/"));
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("Conta de utilizador bloqueada.");
                return RedirectToPage("./Lockout");
            }
            else
            {
                _logger.LogWarning("Código de recuperação inválido introduzido para o utilizador com o ID '{UserId}' ", user.Id);
                ModelState.AddModelError(string.Empty, "Código de recuperação inválido introduzido.");
                return Page();
            }
        }
    }
}
