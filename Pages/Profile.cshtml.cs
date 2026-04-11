using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MoonlightSquad.Class.BLL;
using System.ComponentModel.DataAnnotations;

namespace MoonlightSquad.Pages.Pages;

[Authorize]
public class ProfileModel : PageModel
{
    private readonly UserService _userService;

    public ProfileModel(UserService userService)
    {
        _userService = userService;
    }

    public string Username { get; set; } = string.Empty;
    public string? CurrentProfilePicture { get; set; }

    [BindProperty]
    public IFormFile? ProfilePictureFile { get; set; }

    [BindProperty]
    [StringLength(500, ErrorMessage = "Maximum 500 caractères.")]
    public string? AboutMe { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userService.GetByUsernameAsync(User.Identity!.Name!);
        if (user == null) return NotFound();

        Username = user.Username;
        CurrentProfilePicture = user.ProfilePicture;
        AboutMe = user.AboutMe;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            var u = await _userService.GetByUsernameAsync(User.Identity!.Name!);
            Username = u?.Username ?? string.Empty;
            CurrentProfilePicture = u?.ProfilePicture;
            return Page();
        }

        string? newPictureDataUrl = null;
        if (ProfilePictureFile != null)
        {
            if (ProfilePictureFile.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError(nameof(ProfilePictureFile), "L'image ne doit pas dépasser 2 Mo.");
                var u = await _userService.GetByUsernameAsync(User.Identity!.Name!);
                Username = u?.Username ?? string.Empty;
                CurrentProfilePicture = u?.ProfilePicture;
                return Page();
            }

            using var ms = new MemoryStream();
            await ProfilePictureFile.CopyToAsync(ms);
            var base64 = Convert.ToBase64String(ms.ToArray());
            newPictureDataUrl = $"data:{ProfilePictureFile.ContentType};base64,{base64}";
        }

        await _userService.UpdateProfileAsync(User.Identity!.Name!, AboutMe, newPictureDataUrl);

        TempData["Success"] = "Profil mis à jour avec succès.";
        return RedirectToPage();
    }
}
