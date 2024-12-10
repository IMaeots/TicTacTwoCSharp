using Data.Repositories;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Configurations
{
    public class DeleteModel(IConfigRepository configRepository) : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public required string Id { get; set; }
        private GameConfiguration Configuration { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            Configuration = await configRepository.GetConfigurationByNameAsync(Id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await configRepository.DeleteConfigAsync(Id);
            return RedirectToPage("./Index");
        }
    }
}
