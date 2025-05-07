using Common.Entities;
using Data.Repositories;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Play
{
    public class PlayGameModel(IGameRepository gameRepository) : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public required string GameName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Password { get; set; }

        [BindProperty(SupportsGet = true)]
        public EGameAction? Action { get; set; }

        [BindProperty]
        public string? SelectedMarker { get; set; }

        [BindProperty]
        public EGamePiece? UserPlayerType { get; set; }

        public Game Game { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (!ModelState.IsValid) return Page();
            if (await LoadingGameFailed()) return NotFound();
            if (Game.State.GameOutcome != EGameOutcome.None) return ToResult();

            UserPlayerType = Game.IsPasswordNeeded() ? Game.GetPlayerTypeByPassword(Password) : EGamePiece.Empty;
            if (!string.IsNullOrEmpty(Password) && UserPlayerType == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid password.");
                return Page();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? move, bool? bot, bool? deselect)
        {
            if (!ModelState.IsValid) return Page();
            if (await LoadingGameFailed()) return NotFound();
            if (bot != null)
            {
                GameBot.MakeMove(Game);
                return await MoveMade();
            }

            UserPlayerType = Game.IsPasswordNeeded() ? Game.GetPlayerTypeByPassword(Password) : EGamePiece.Empty;
            if (!string.IsNullOrEmpty(Password) && UserPlayerType == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid password.");
                return Page();
            }

            if (string.IsNullOrEmpty(move)) return Page();

            if (deselect == true && !string.IsNullOrEmpty(SelectedMarker))
            {
                SelectedMarker = null;
                return Page();
            }

            var (x, y) = ParseMove(move);

            Action ??= EGameAction.PlaceMarker;
            switch (Action)
            {
                case EGameAction.PlaceMarker:
                    if (!Game.CanPlaceMarker())
                    {
                        ModelState.AddModelError(string.Empty, "You have placed all your markers.");
                        return Page();
                    }
                    if (!Game.PlaceMarker(x, y))
                    {
                        ModelState.AddModelError(string.Empty, "Invalid move. Cell is already occupied.");
                        return Page();
                    }

                    break;
                case EGameAction.MoveGrid:
                    if (!Game.MoveGrid(x, y))
                    {
                        ModelState.AddModelError(string.Empty, "Unable to move grid there.");
                        return Page();
                    }

                    break;
                case EGameAction.MoveMarker:
                {
                    if (string.IsNullOrEmpty(SelectedMarker))
                    {
                        if (Game.CanMoveThatMarker(x, y)) SelectedMarker = move;
                        else ModelState.AddModelError(string.Empty, "Invalid move. You cannot select that spot.");
                        return Page();
                    }

                    var (sx, sy) = ParseMove(SelectedMarker);
                    if (!Game.MoveMarker(sx, sy, x, y))
                    {
                        ModelState.AddModelError(string.Empty, "Unable to move that marker there.");
                        return Page();
                    }

                    SelectedMarker = null;
                    break;
                }
            }

            return await MoveMade();
        }

        private async Task<bool> LoadingGameFailed()
        {
            try
            {
                Game = await gameRepository.GetSavedGameByNameAsync(GameName);
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }

        private RedirectToPageResult ToResult() => RedirectToPage("/Play/Result", new { gameName = GameName });

        private async Task<IActionResult> MoveMade()
        {
            Game.UpdateGameOutcome();
            await gameRepository.SaveGameStateAsync(Game);

            return Game.State.GameOutcome != EGameOutcome.None ? ToResult() : Page();
        }

        public (int x, int y) ParseMove(string move)
        {
            var parts = move.Split(',');
            if (parts.Length != 2 || !int.TryParse(parts[0], out var x) || !int.TryParse(parts[1], out var y))
                throw new ArgumentException("Invalid move format. Expected format is 'x,y'.");
            return (x, y);
        }
    }
}
