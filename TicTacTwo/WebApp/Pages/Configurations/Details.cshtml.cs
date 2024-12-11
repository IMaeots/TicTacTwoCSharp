using Data.Repositories;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Configurations;

public class DetailsModel(IConfigRepository configRepository) : PageModel
{
    public GameConfiguration Configuration { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public required string ConfigName { get; set; }

    public async Task OnGetAsync(string configName)
    {
        ConfigName = configName;
        Configuration = await configRepository.GetConfigurationByNameAsync(ConfigName);
    }
}
