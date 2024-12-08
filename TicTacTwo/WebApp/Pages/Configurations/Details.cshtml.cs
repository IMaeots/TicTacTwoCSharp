using Data.Context;
using Data.Models.db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Configurations
{
    public class DetailsModel(GameDbContext context) : PageModel
    {
        public SaveGameConfiguration SaveGameConfiguration { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saveGameConfiguration = await context.SavedGameConfigurations.FirstOrDefaultAsync(m => m.Id == id);

            if (saveGameConfiguration is null) return NotFound();

            SaveGameConfiguration = saveGameConfiguration;
            return Page();
        }
    }
}
