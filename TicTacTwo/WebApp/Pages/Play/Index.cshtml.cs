using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Play
{
    public class PlayGameModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public required string GameName { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string? Password { get; set; }
        
        public void OnGet(string gameName, string? password)
        {
            GameName = gameName;
            Password = password;
        }
        
        public IActionResult OnPost(string password)
        {
            Password = password;

            return Page();
        }
    }
}
