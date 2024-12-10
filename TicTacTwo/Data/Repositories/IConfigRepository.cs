using GameLogic;

namespace Data.Repositories;

public interface IConfigRepository
{
    Task<List<string>> GetConfigurationNamesAsync();
    Task<GameConfiguration> GetConfigurationByNameAsync(string name);
    Task SaveConfigAsync(GameConfiguration newConfig);
    Task DeleteConfigAsync(string name);
}
