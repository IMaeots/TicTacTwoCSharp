using System.Text.Json;
using Data.Context;
using Data.Models.db;
using GameLogic;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Config;

public class ConfigRepositoryDb(GameDbContext dbContext) : IConfigRepository
{
    public async Task<List<string>> GetConfigurationNamesAsync()
    {
        await CheckAndCreateInitialConfigAsync();

        return await dbContext.SavedGameConfigurations
            .Select(config => config.Name)
            .ToListAsync();
    }

    public async Task<GameConfiguration> GetConfigurationByNameAsync(string name)
    {
        var savedConfiguration = await dbContext.SavedGameConfigurations
            .FirstOrDefaultAsync(config => config.Name == name);

        if (savedConfiguration == null)
            throw new KeyNotFoundException($"Configuration {name} not found");

        var gameConfiguration = JsonSerializer.Deserialize<GameConfiguration>(savedConfiguration.JsonConfiguration);
        if (gameConfiguration == null)
            throw new InvalidOperationException($"Configuration for {name} could not be deserialized.");

        return gameConfiguration;
    }

    public async Task SaveConfigAsync(GameConfiguration newConfig)
    {
        var jsonData = JsonSerializer.Serialize(newConfig);
        var configuration = new SaveGameConfiguration
        {
            Name = newConfig.Name,
            JsonConfiguration = jsonData
        };

        await dbContext.SavedGameConfigurations.AddAsync(configuration);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteConfigAsync(string name)
    {
        var configuration = await dbContext.SavedGameConfigurations
            .FirstOrDefaultAsync(config => config.Name == name);

        if (configuration == null)
            throw new KeyNotFoundException($"Configuration {name} not found");

        dbContext.SavedGameConfigurations.Remove(configuration);
        await dbContext.SaveChangesAsync();
    }

    private async Task CheckAndCreateInitialConfigAsync()
    {
        if (await dbContext.SavedGameConfigurations.AnyAsync()) return;

        var defaultGameConfigurations = GameConfiguration.GetDefaultGameConfigurations();
        foreach (var config in defaultGameConfigurations)
        {
            await SaveConfigAsync(config);
        }
    }
}
