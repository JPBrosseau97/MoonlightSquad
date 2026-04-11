using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MoonlightSquad.Class.BLL;
using MoonlightSquad.Class.DAL;

namespace MoonlightSquad.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly NewsService _newsService;

        public List<News> News { get; set; } = new();

        public IndexModel(ILogger<IndexModel> logger, NewsService newsService)
        {
            _logger = logger;
            _newsService = newsService;
        }

        public async Task OnGetAsync()
        {
            News = await _newsService.GetAllActiveAsync();
        }
    }
}
