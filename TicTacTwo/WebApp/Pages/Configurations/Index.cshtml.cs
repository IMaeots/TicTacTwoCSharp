using Data.Context;
using Data.Models.db;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Configurations
{
    public class IndexModel(GameDbContext context) : PageModel
    {
        public IList<SaveGameConfiguration> SaveGameConfiguration { get; set; } = default!;

        public async Task OnGetAsync()
        {
            SaveGameConfiguration = await context.SavedGameConfigurations.ToListAsync();
        }
    }
}
