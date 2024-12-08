using Data.Context;
using Data.Models.db;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Games
{
    public class IndexModel(GameDbContext context) : PageModel
    {
        public IList<SaveGame> SaveGame { get; set; } = default!;

        public async Task OnGetAsync()
        {
            SaveGame = await context.SavedGames
                .Include(s => s.Configuration).ToListAsync();
        }
    }
}
