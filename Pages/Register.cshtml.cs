using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MoonlightSquad.Class.BLL;
using System.ComponentModel.DataAnnotations;

namespace MoonlightSquad.Pages.Pages;

public class RegisterModel : PageModel
{
    private readonly UserService _userService;

    public RegisterModel(UserService userService)
    {
        _userService = userService;
    }

    [BindProperty]
    [Required(ErrorMessage = "Le nom d'utilisateur est requis.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Entre 3 et 50 caractères.")]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Le mot de passe est requis.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Minimum 6 caractères.")]
    public string Password { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "La confirmation est requise.")]
    [Compare(nameof(Password), ErrorMessage = "Les mots de passe ne correspondent pas.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var success = await _userService.RegisterAsync(Username, Password);
        if (!success)
        {
            ModelState.AddModelError(nameof(Username), "Ce nom d'utilisateur est déjà pris.");
            return Page();
        }

        return RedirectToPage("/Login");
    }
}
