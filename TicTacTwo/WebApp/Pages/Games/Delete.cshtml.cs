using Data.Context;
using Data.Models.db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Games
{
    public class DeleteModel(GameDbContext context) : PageModel
    {
        [BindProperty]
        public SaveGame SaveGame { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saveGame = await context.SavedGames.FirstOrDefaultAsync(m => m.Id == id);

            if (saveGame is null) return NotFound();
            SaveGame = saveGame;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var savegame = await context.SavedGames.FindAsync(id);
            if (savegame == null) return RedirectToPage("./Index");

            SaveGame = savegame;
            context.SavedGames.Remove(SaveGame);
            await context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
