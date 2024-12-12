using Common.Entities;
using Data.Repositories;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Play;

public class ResultModel(IGameRepository gameRepository) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string GameName { get; set; }

    public Game? Game { get; set; }
    public string WinnerMessage { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            Game = await gameRepository.GetSavedGameByNameAsync(GameName);
        }
        catch (Exception)
        {
            if (Game == null) return RedirectToPage("/Index");
        }

        if (Game.State.GameOutcome == EGameOutcome.None)
            return RedirectToPage("/Index");

        WinnerMessage = Game.State.GameOutcome switch
        {
            EGameOutcome.Player1Won => "Game Over! Player 1 wins!",
            EGameOutcome.Player2Won => "Game Over! Player 2 wins!",
            EGameOutcome.Draw => "Game Over! It's a draw!",
            _ => WinnerMessage
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        
        try
        {
            Game = await gameRepository.GetSavedGameByNameAsync(GameName);
        }
        catch (Exception)
        {
            if (Game == null) return RedirectToPage("/Index");
        }
        
        try
        {
            if (Game != null) await gameRepository.DeleteGameAsync(Game);
            return RedirectToPage("/Index");
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "There was an issue deleting the game. Please try again later.");
        }

        return Page();
    }
}
