using Data.Repositories;
using GameLogic;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Games
{
    public class SavedGamesModel(IGameRepository gameRepository) : PageModel
    {
        public IList<Game> Games { get; set; } = new List<Game>();

        public async Task OnGetAsync()
        {
            var gameNames = await gameRepository.GetSavedGamesNamesAsync();
            foreach (var gameName in gameNames)
            {
                Games.Add(await gameRepository.GetSavedGameByNameAsync(gameName));
            }
        }
    }
}
