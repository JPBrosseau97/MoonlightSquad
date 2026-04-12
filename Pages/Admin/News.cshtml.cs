using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MoonlightSquad.Class.BLL;
using MoonlightSquad.Class.DAL;

namespace MoonlightSquad.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class NewsModel : PageModel
    {
        private readonly NewsService _newsService;

        public List<News> NewsList { get; set; } = new();
        public News? EditingNews { get; set; }
        public bool ShowForm { get; set; }

        [BindProperty]
        public int? EditId { get; set; }

        [BindProperty]
        public string? Title { get; set; }

        [BindProperty]
        public string? NewsContent { get; set; }

        [BindProperty]
        public string? ImageUrl { get; set; }

        [BindProperty]
        public bool IsActive { get; set; } = true;

        [BindProperty]
        public int Order { get; set; } = 0;

        public NewsModel(NewsService newsService)
        {
            _newsService = newsService;
        }

        public async Task OnGetAsync(int? editId = null)
        {
            NewsList = await _newsService.GetAllAsync();

            if (editId.HasValue)
            {
                EditingNews = await _newsService.GetByIdAsync(editId.Value);
                if (EditingNews != null)
                {
                    EditId = EditingNews.Id;
                    Title = EditingNews.Title;
                    NewsContent = EditingNews.Content;
                    ImageUrl = EditingNews.ImageUrl;
                    IsActive = EditingNews.IsActive;
                    Order = EditingNews.Order;
                    ShowForm = true;
                }
            }
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(NewsContent))
            {
                return RedirectToPage();
            }

            if (EditId.HasValue)
            {
                var news = new News
                {
                    Id = EditId.Value,
                    Title = Title!,
                    Content = NewsContent!,
                    ImageUrl = ImageUrl,
                    IsActive = IsActive,
                    Order = Order
                };
                await _newsService.UpdateAsync(news);
            }
            else
            {
                var userId = int.Parse(User.FindFirst("UserId")!.Value);
                var news = new News
                {
                    Title = Title!,
                    Content = NewsContent!,
                    ImageUrl = ImageUrl,
                    IsActive = IsActive,
                    Order = Order,
                    CreatedByUserId = userId
                };
                await _newsService.CreateAsync(news);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _newsService.DeleteAsync(id);
            return RedirectToPage();
        }

        public IActionResult OnPostCancel()
        {
            return RedirectToPage();
        }
    }
}
