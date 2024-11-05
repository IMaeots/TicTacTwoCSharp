using System.Text.Json;
using Data.Context;
using Data.Models;
using GameBrain;

namespace Data.Repositories.Config;

public class ConfigRepositoryDb(GameDbContext dbContext) : IConfigRepository
{
    public List<string> GetConfigurationNames()
    {
        return dbContext.Configurations.Select(config => config.Name.ToString()).ToList();
    }

    public GameConfiguration? GetConfigurationByName(string name)
    {
        var configuration = dbContext.Configurations.FirstOrDefault(config => config.Name.ToString() == name);
        return configuration != null 
            ? JsonSerializer.Deserialize<GameConfiguration>(configuration.JsonConfiguration) 
            : null;
    }

    public void SaveConfig(GameConfiguration newConfig)
    {
        var jsonData = JsonSerializer.Serialize(newConfig);
        var configuration = new SaveGameConfiguration
        {
            Name = newConfig.Name,
            JsonConfiguration = jsonData
        };

        dbContext.Configurations.Add(configuration);
        dbContext.SaveChanges();
    }
}
