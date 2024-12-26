// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using LibSpace_Aspnet.Data;
using LibSpace_Aspnet.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;



namespace LibSpace_Aspnet.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext dbContext,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _dbContext = dbContext;
            _roleManager = roleManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required]
            [Display(Name = "Nome")]
            public string Nome { get; set; }

            [Required]
            [Display(Name = "Apelido")]
            public string Apelido { get; set; }
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

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


            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "A {0} deve ter entre {2} e {1} caracteres.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
                ErrorMessage = "A senha deve ter pelo menos 8 caracteres, incluindo letras maiúsculas, letras minúsculas, números e caracteres especiais.")]
            public string Password { get; set; }


            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Role")]
            public string Role { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ViewData["Roles"] = _roleManager.Roles.ToList();
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            // Define o URL de retorno padrão se não for fornecido
            returnUrl ??= Url.Content("~/");
            // Obtém os esquemas de autenticação externa disponíveis
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // Cria uma nova instância de utilizador
                var user = CreateUser();
                user.PhoneNumber = Input.PhoneNumber;

                // Define o nome de utilizador e o email
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                // Cria o utilizador com a senha fornecida
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Utilizador criou uma nova conta com password.");

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

                    // Verifica se o código postal já existe na base de dados
                    var codigoPostal = await _dbContext.CodigoPostals.FirstOrDefaultAsync(cp => cp.EndCodPostal == Input.CodigoPostal);

                    if (codigoPostal == null)
                    {
                        // Se não existir, cria um novo registo de código postal
                        codigoPostal = new CodigoPostal
                        {
                            EndCodPostal = Input.CodigoPostal,
                            EndLocalidade = Input.Cidade
                        };
                        _dbContext.CodigoPostals.Add(codigoPostal);
                    }

                    // Cria um novo perfil para o utilizador
                    var perfil = new Perfil
                    {
                        EndMorada = $"{Input.Morada}, {Input.Cidade}",
                        EndCodPostal = Input.CodigoPostal,
                        NomePerfil = Input.Nome,
                        DataNascimentoPerfil = DateOnly.FromDateTime(Input.Birthday),
                        Apelido = Input.Apelido,
                        AspNetUserId = user.Id,
                        ImgPerfil = null
                    };

                    // Adiciona o perfil à base de dados
                    _dbContext.Perfils.Add(perfil);
                    await _dbContext.SaveChangesAsync();

                    // Gera o token de confirmação de email
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    // Cria o URL de callback para confirmação de email
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    // Envia o email de confirmação
                    await _emailSender.SendEmailAsync(Input.Email, "Confirme o seu email",
                        $"Por favor, confirme a sua conta clicando <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>aqui</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        // Redireciona para a página de confirmação de registo
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        // Inicia sessão automaticamente
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }

                // Adiciona erros ao ModelState se a criação do utilizador falhar
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Recarrega a lista de papéis disponíveis se algo falhar
            ViewData["Roles"] = _roleManager.Roles.ToList();
            // Se ocorrer um erro, reexibe o formulário
            return Page();
        }


        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
