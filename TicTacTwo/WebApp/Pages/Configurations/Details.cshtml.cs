using Data.Repositories;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Configurations;

public class DetailsModel(IConfigRepository configRepository) : PageModel
{
    public GameConfiguration Configuration { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public required string Id { get; set; }

    public async Task OnGetAsync()
    {
        Configuration = await configRepository.GetConfigurationByNameAsync(Id);
    }
}
