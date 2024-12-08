using Data.Context;
using Data.Models.db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Games
{
    public class DetailsModel(GameDbContext context) : PageModel
    {
        public SaveGame SaveGame { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var savegame = await context.SavedGames.FirstOrDefaultAsync(m => m.Id == id);

            if (savegame is null) return NotFound();

            SaveGame = savegame;
            return Page();
        }
    }
}
