using Data.Models.db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.Play;

public class Setup(Data.Context.GameDbContext context) : PageModel
{
    [BindProperty]
    public SaveGame SaveGame { get; set; } = default!;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        context.SavedGames.Add(SaveGame);
        await context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
