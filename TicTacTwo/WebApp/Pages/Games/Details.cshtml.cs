using Data.Repositories;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Games;

public class DetailsModel(IGameRepository gameRepository) : PageModel
{
    public Game Game { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public required string Id { get; set; }

    public async Task OnGetAsync()
    {
        Game = await gameRepository.GetSavedGameByNameAsync(Id);
    }
}
