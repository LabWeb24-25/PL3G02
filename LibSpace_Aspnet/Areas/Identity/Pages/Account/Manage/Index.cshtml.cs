// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using LibSpace_Aspnet.Data;
using LibSpace_Aspnet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LibSpace_Aspnet.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

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

            [Display(Name = "Foto de Perfil Atual")]
            public string ProfilePhotoPath { get; set; }

            [Display(Name = "Nova Foto de Perfil")]
            [DataType(DataType.Upload)]
            public IFormFile ProfilePhoto { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var perfil = await _dbContext.Perfils.SingleOrDefaultAsync(p => p.AspNetUserId == user.Id);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Nome = perfil.NomePerfil,
                Apelido = perfil.Apelido,
                Morada = perfil.EndMorada,
                CodigoPostal = perfil.EndCodPostal,
                Birthday = perfil.DataNascimentoPerfil.ToDateTime(TimeOnly.MinValue),
                ProfilePhotoPath = string.IsNullOrEmpty(perfil.ImgPerfil) ? "wwwroot/images/profiles/deafult_lab.webp" : perfil.ImgPerfil
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            var perfil = await _dbContext.Perfils.SingleOrDefaultAsync(p => p.AspNetUserId == user.Id);

            if (perfil == null)
            {
                // Perfil não encontrado, exiba uma mensagem de erro
                ModelState.AddModelError(string.Empty, "Perfil não encontrado.");
                return Page();
            }

            if (Input.Nome != perfil.NomePerfil)
            {
                perfil.NomePerfil = Input.Nome;
            }
            if (Input.Apelido != perfil.Apelido)
            {
                perfil.Apelido = Input.Apelido;
            }
            if (Input.Morada != perfil.EndMorada)
            {
                perfil.EndMorada = Input.Morada;
            }
            if (Input.CodigoPostal != perfil.EndCodPostal)
            {
                var codigoPostal = await _dbContext.CodigoPostals.FirstOrDefaultAsync(cp => cp.EndCodPostal == Input.CodigoPostal);

                if (codigoPostal == null)
                {
                    // Se não existir, criar um novo
                    codigoPostal = new CodigoPostal
                    {
                        EndCodPostal = Input.CodigoPostal,
                        EndLocalidade = "Localidade"
                    };
                    _dbContext.CodigoPostals.Add(codigoPostal);
                }
                perfil.EndCodPostal = Input.CodigoPostal;
            }
            if (Input.Birthday != perfil.DataNascimentoPerfil.ToDateTime(TimeOnly.MinValue))
            {
                perfil.DataNascimentoPerfil = DateOnly.FromDateTime(Input.Birthday);
            }

            if (Input.ProfilePhoto != null)
            {
                var filePath = Path.Combine("wwwroot/images/profiles", Input.ProfilePhoto.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.ProfilePhoto.CopyToAsync(stream);
                }
                perfil.ImgPerfil = $"/images/profiles/{Input.ProfilePhoto.FileName}";
            }

            // Atualizar o perfil no banco de dados
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Logar a exceção ou adicionar um erro ao ModelState
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao guardar as alterações: " + ex.Message);
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "O seu perfil foi atualizado com sucesso";
            return RedirectToPage();
        }
    }
}