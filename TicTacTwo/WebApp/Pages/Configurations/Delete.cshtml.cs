using Data.Repositories;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Configurations
{
    public class DeleteModel(IConfigRepository configRepository) : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public required string ConfigName { get; set; }
        private GameConfiguration Configuration { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string configName)
        {
            ConfigName = configName;
            Configuration = await configRepository.GetConfigurationByNameAsync(ConfigName);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await configRepository.DeleteConfigAsync(ConfigName);
            return RedirectToPage("./Index");
        }
    }
}
