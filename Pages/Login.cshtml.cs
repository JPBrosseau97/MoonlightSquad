using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MoonlightSquad.Class.BLL;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace MoonlightSquad.Pages.Pages;

public class LoginModel : PageModel
{
    private readonly UserService _userService;

    public LoginModel(UserService userService)
    {
        _userService = userService;
    }

    [BindProperty]
    [Required(ErrorMessage = "Le nom d'utilisateur est requis.")]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Le mot de passe est requis.")]
    public string Password { get; set; } = string.Empty;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var user = await _userService.ValidateAsync(Username, Password);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Nom d'utilisateur ou mot de passe incorrect.");
            return Page();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var properties = new AuthenticationProperties { IsPersistent = true };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            properties);

        return RedirectToPage("/Index");
    }
}
