using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MoonlightSquad.Class.BLL;

namespace MoonlightSquad.Pages.Pages;

public class NewsEditModel : PageModel
{
    private readonly NewsService _newsService;

    public NewsEditModel(NewsService newsService)
    {
        _newsService = newsService;
    }

    public List<string> Categories { get; set; } = new();

    public async Task OnGetAsync()
    {
        Categories = (await _newsService.GetAllCategoriesAsync())
            .Select(c => c.CategoryName)
            .ToList();
    }
}
