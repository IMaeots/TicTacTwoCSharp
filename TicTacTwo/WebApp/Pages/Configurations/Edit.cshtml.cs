using Data.Context;
using Data.Models.db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Configurations
{
    public class EditModel(GameDbContext context) : PageModel
    {
        [BindProperty]
        public SaveGameConfiguration SaveGameConfiguration { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saveGameConfiguration = await context.SavedGameConfigurations.FirstOrDefaultAsync(m => m.Id == id);
            if (saveGameConfiguration == null)
            {
                return NotFound();
            }

            SaveGameConfiguration = saveGameConfiguration;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            context.Attach(SaveGameConfiguration).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaveGameConfigurationExists(SaveGameConfiguration.Id)) return NotFound();
                throw;
            }

            return RedirectToPage("./Index");
        }

        private bool SaveGameConfigurationExists(int id)
        {
            return context.SavedGameConfigurations.Any(e => e.Id == id);
        }
    }
}
