using Data.Repositories;
using GameLogic;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Configurations
{
    public class ConfigurationsModel(IConfigRepository configRepository) : PageModel
    {
        public IList<GameConfiguration> Configurations { get; set; } = new List<GameConfiguration>();

        public async Task OnGetAsync()
        {
            var configNames = await configRepository.GetConfigurationNamesAsync();
            foreach (var configName in configNames)
            {
                Configurations.Add(await configRepository.GetConfigurationByNameAsync(configName));
            }
        }
    }
}
