using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Play;

public class PlayGameModel : PageModel
{
    public required string GameName { get; set; }
    
    public void OnGet(string gameName)
    {
        GameName = gameName;
    }
}
