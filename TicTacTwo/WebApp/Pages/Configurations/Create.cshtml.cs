using Data.Repositories;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Configurations
{
    public class CreateModel(IConfigRepository configRepository) : PageModel
    {
        [BindProperty]
        public GameConfiguration Configuration { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            
            var nameValidation = GameConfigurationValidator.ValidateInputAsAlphanumeric(Configuration.Name);
            if (!string.IsNullOrEmpty(nameValidation))
            {
                ModelState.AddModelError(string.Empty, nameValidation);
            }
            
            var winConditionValidation = GameConfigurationValidator.ValidateWinCondition(
                Configuration.WinCondition,
                Configuration.GridHeight,
                Configuration.GridWidth
            );
            if (!string.IsNullOrEmpty(winConditionValidation))
            {
                ModelState.AddModelError(string.Empty, winConditionValidation);
            }
            
            var boardWidthValidation = GameConfigurationValidator.ValidateBoardWidth(Configuration.BoardWidth);
            if (!string.IsNullOrEmpty(boardWidthValidation))
            {
                ModelState.AddModelError(string.Empty, boardWidthValidation);
            }

            var boardHeightValidation = GameConfigurationValidator.ValidateBoardHeight(Configuration.BoardHeight);
            if (!string.IsNullOrEmpty(boardHeightValidation))
            {
                ModelState.AddModelError(string.Empty, boardHeightValidation);
            }

            var gridWidthValidation = GameConfigurationValidator.ValidateGridWidth(Configuration.GridWidth, Configuration.BoardWidth);
            if (!string.IsNullOrEmpty(gridWidthValidation))
            {
                ModelState.AddModelError(string.Empty, gridWidthValidation);
            }

            var gridHeightValidation = GameConfigurationValidator.ValidateGridHeight(Configuration.GridHeight, Configuration.BoardHeight);
            if (!string.IsNullOrEmpty(gridHeightValidation))
            {
                ModelState.AddModelError(string.Empty, gridHeightValidation);
            }

            var specialMovesValidation = GameConfigurationValidator.ValidateMoveGridAfterNMoves(Configuration.UnlockSpecialMovesAfterNMoves);
            if (!string.IsNullOrEmpty(specialMovesValidation))
            {
                ModelState.AddModelError(string.Empty, specialMovesValidation);
            }

            var markersValidation = GameConfigurationValidator.ValidateMarkers(Configuration.NumberOfMarkers, Configuration.WinCondition);
            if (!string.IsNullOrEmpty(markersValidation))
            {
                ModelState.AddModelError(string.Empty, markersValidation);
            }

            var startingXValidation = GameConfigurationValidator.ValidateStartingGridXPosition(
                Configuration.StartingGridXPosition,
                Configuration.BoardWidth,
                Configuration.GridWidth
            );
            if (!string.IsNullOrEmpty(startingXValidation))
            {
                ModelState.AddModelError(string.Empty, startingXValidation);
            }

            var startingYValidation = GameConfigurationValidator.ValidateStartingGridYPosition(
                Configuration.StartingGridYPosition,
                Configuration.BoardHeight,
                Configuration.GridHeight
            );
            if (!string.IsNullOrEmpty(startingYValidation))
            {
                ModelState.AddModelError(string.Empty, startingYValidation);
            }
            
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            await configRepository.SaveConfigAsync(Configuration);
            return RedirectToPage("./Index");
        }
    }
}
