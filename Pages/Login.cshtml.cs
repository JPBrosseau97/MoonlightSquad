using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace MoonlightSquad.Pages.Pages;

public class LoginModel : PageModel
{
    public void OnGet()
    {
        
    }
[Required]
public string Username { get; set; } =string.Empty;
[Required]
public string Password { get; set; }= string.Empty;
    public IActionResult OnPost(string username, string password)
    {
        // Ici, vous devriez vérifier les informations d'identification de l'utilisateur
        // Par exemple, en les comparant à une base de données ou à une liste d'utilisateurs

        if (username == "admin" && password == "password") // Remplacez par votre logique d'authentification
        {
            // Si les informations d'identification sont correctes, connectez l'utilisateur
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true // Permet de garder l'utilisateur connecté après la fermeture du navigateur
            };

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToPage("/Index"); // Redirige vers la page d'accueil après la connexion
        }

        // Si les informations d'identification sont incorrectes, affichez un message d'erreur ou redirigez vers la page de connexion
        ModelState.AddModelError(string.Empty, "Nom d'utilisateur ou mot de passe incorrect.");
        return Page();
    }
}
