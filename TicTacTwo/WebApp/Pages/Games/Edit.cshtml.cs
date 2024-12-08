using Data.Context;
using Data.Models.db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Games
{
    public class EditModel(GameDbContext context) : PageModel
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
            if (saveGame == null)
            {
                return NotFound();
            }

            SaveGame = saveGame;
            ViewData["ConfigurationId"] = new SelectList(context.SavedGameConfigurations, "Id", "JsonConfiguration");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            context.Attach(SaveGame).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaveGameExists(SaveGame.Id)) return NotFound();
                throw;
            }

            return RedirectToPage("./Index");
        }

        private bool SaveGameExists(int id)
        {
            return context.SavedGames.Any(e => e.Id == id);
        }
    }
}
