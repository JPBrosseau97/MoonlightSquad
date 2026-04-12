using MoonlightSquad.Class.DAL;
using Microsoft.EntityFrameworkCore;

namespace MoonlightSquad.Class.BLL
{
    public class NewsService
    {
        private readonly AppDbContext _db;

        public NewsService(AppDbContext db)
        {
            _db = db;
        }

        // Récupérer toutes les nouvelles actives triées par ordre d'affichage
        public Task<List<News>> GetAllActiveAsync()
        {
            return _db.News
                .Where(n => n.IsActive)
                .OrderBy(n => n.Order)
                .ThenByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        // Récupérer toutes les nouvelles (incluant les inactives) pour l'admin
        public Task<List<News>> GetAllAsync()
        {
            return _db.News
                .OrderBy(n => n.Order)
                .ThenByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        // Récupérer une nouvelle par ID
        public Task<News?> GetByIdAsync(int id)
        {
            return _db.News.FirstOrDefaultAsync(n => n.Id == id);
        }

        // Créer une nouvelle
        public async Task<int> CreateAsync(News news)
        {
            news.CreatedAt = DateTime.Now;
            news.UpdatedAt = null;
            _db.News.Add(news);
            await _db.SaveChangesAsync();
            return news.Id;
        }

        // Modifier une nouvelle
        public async Task UpdateAsync(News news)
        {
            var existing = await _db.News.FirstOrDefaultAsync(n => n.Id == news.Id);
            if (existing != null)
            {
                existing.Title = news.Title;
                existing.Content = news.Content;
                existing.ImageUrl = news.ImageUrl;
                existing.IsActive = news.IsActive;
                existing.Order = news.Order;
                existing.UpdatedAt = DateTime.Now;
                _db.News.Update(existing);
                await _db.SaveChangesAsync();
            }
        }

        // Supprimer une nouvelle
        public async Task DeleteAsync(int id)
        {
            var news = await _db.News.FirstOrDefaultAsync(n => n.Id == id);
            if (news != null)
            {
                _db.News.Remove(news);
                await _db.SaveChangesAsync();
            }
        }

        // Obtenir le nombre de nouvelles
        public Task<int> GetCountAsync()
        {
            return _db.News.CountAsync();
        }

        public Task<List<News>> GetByCategoryAsync(int categoryId)
        {
            return _db.News
                .Where(n => n.IdNewsCategory == categoryId && n.IsActive)
                .OrderBy(n => n.Order)
                .ThenByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public Task<List<NewsCategory>> GetAllCategoriesAsync()
        {
            return _db.NewsCategories.OrderBy(c => c.CategoryName).ToListAsync();
        }
    }
}
