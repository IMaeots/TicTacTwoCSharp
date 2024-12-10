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
        public required string Id { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var game = await gameRepository.GetSavedGameByNameAsync(Id);

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

            var originalGame = await gameRepository.GetSavedGameByNameAsync(Id);
            await gameRepository.EditGameNameAsync(originalGame, NewName);
            
            return RedirectToPage("./Index");
        }
    }
}
