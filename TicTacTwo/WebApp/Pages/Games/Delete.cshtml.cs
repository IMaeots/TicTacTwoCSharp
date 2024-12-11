using Data.Repositories;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Games;

public class DeleteModel(IGameRepository gameRepository) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string GameName { get; set; }

    public void OnGet(string gameName)
    {
        GameName = gameName;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var game = await gameRepository.GetSavedGameByNameAsync(GameName);
        await gameRepository.DeleteGameAsync(game);
        return RedirectToPage("./Index");
    }
}
