using Data.Repositories;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Games;

public class DeleteModel(IGameRepository gameRepository) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string Id { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        var game = await gameRepository.GetSavedGameByNameAsync(Id);
        await gameRepository.DeleteGameAsync(game);
        return RedirectToPage("./Index");
    }
}
