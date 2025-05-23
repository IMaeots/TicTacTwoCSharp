using Common.Entities;
using Data.Repositories;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Play
{
    public class SetupModel(IGameRepository gameRepository, IConfigRepository configRepository) : PageModel
    {
        [BindProperty]
        public required string GameName { get; set; }

        [BindProperty]
        public required string ConfigurationName { get; set; }

        [BindProperty]
        public string? PasswordP1 { get; set; }

        [BindProperty]
        public string? PasswordP2 { get; set; }

        public required SelectList ConfigurationNames { get; set; }

        public async Task OnGetAsync()
        {
            await LoadConfigurationNamesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadConfigurationNamesAsync();
            if (!ModelState.IsValid) return Page();

            var errorMessage = GameConfigurationValidator.ValidateInputAsAlphanumeric(GameName);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                return Page();
            }

            GameConfiguration selectedConfiguration;
            try
            {
                selectedConfiguration = await configRepository.GetConfigurationByNameAsync(ConfigurationName);
            }
            catch (KeyNotFoundException)
            {
                ModelState.AddModelError(string.Empty, "Selected game configuration no longer exists.");
                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unable to load game configuration. Please try again.");
                return Page();
            }

            if (selectedConfiguration.Mode is EGameMode.SinglePlayer && PasswordP1 == null)
            {
                ModelState.AddModelError(string.Empty, "Single player mode requires password for player 1.");
                return Page();
            }
            
            if (selectedConfiguration.Mode is EGameMode.OnlineTwoPlayer && (PasswordP1 == null || PasswordP2 == null))
            {
                ModelState.AddModelError(string.Empty, "Online Two player mode requires passwords for both players.");
                return Page();
            }

            if (PasswordP1 != null)
            {
                errorMessage = GameConfigurationValidator.ValidateInputAsAlphanumeric(PasswordP1);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    ModelState.AddModelError(string.Empty, errorMessage);
                    return Page();
                }
            }

            if (PasswordP2 != null)
            {
                errorMessage = GameConfigurationValidator.ValidateInputAsAlphanumeric(PasswordP2);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    ModelState.AddModelError(string.Empty, errorMessage);
                    return Page();
                }

                if (PasswordP1 == PasswordP2)
                {
                    ModelState.AddModelError(string.Empty, "Passwords can not match.");
                    return Page();
                }
            }

            var gameState = new GameState(selectedConfiguration);
            var newGame = new Game(GameName, selectedConfiguration, gameState, PasswordP1, PasswordP2);
            try
            {
                await gameRepository.SaveNewGameAsync(newGame);
                return RedirectToPage("/Play/Index", new { gameName = GameName });
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Game with that name already exists");
                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unable to create game. Please try again.");
                return Page();
            }
        }

        private async Task LoadConfigurationNamesAsync()
        {
            try
            {
                var configNames = await configRepository.GetConfigurationNamesAsync();
                ConfigurationNames = new SelectList(configNames);
            }
            catch (Exception)
            {
                ConfigurationNames = new SelectList(Array.Empty<string>());
            }
        }
    }
}
