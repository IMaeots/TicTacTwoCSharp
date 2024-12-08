using Data.Context;
using Data.Models.db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Configurations
{
    public class CreateModel(GameDbContext context) : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public SaveGameConfiguration SaveGameConfiguration { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            context.SavedGameConfigurations.Add(SaveGameConfiguration);
            await context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
