using Data.Repositories;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Games
{
    public class EditModel(IGameRepository gameRepository) : PageModel
    {
        [BindProperty]
        public string NewName { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public required string GameName { get; set; }

        public async Task<IActionResult> OnGetAsync(string gameName)
        {
            GameName = gameName;
            var game = await gameRepository.GetSavedGameByNameAsync(GameName);

            NewName = game.Name;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var errorMessage = GameConfigurationValidator.ValidateInputAsAlphanumeric(NewName);
            if (errorMessage != null)
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                return Page();
            }

            if (!ModelState.IsValid) return Page();

            var originalGame = await gameRepository.GetSavedGameByNameAsync(GameName);
            await gameRepository.EditGameNameAsync(originalGame, NewName);
            
            return RedirectToPage("./Index");
        }
    }
}
