using System.Text.Json;
using Common;
using GameLogic;

namespace Data.Repositories.Config;

public class ConfigRepositoryJson : IConfigRepository
{
    public async Task<List<string>> GetConfigurationNamesAsync()
    {
        await CheckAndCreateInitialConfigAsync();

        return Directory
            .GetFiles(Constants.ConfigurationsPath, "*" + Constants.JsonFileExtension)
            .Select(Path.GetFileNameWithoutExtension)
            .Where(name => name != null)
            .Cast<string>()
            .ToList();
    }

    public async Task<GameConfiguration> GetConfigurationByNameAsync(string name)
    {
        var filePath = Path.Combine(Constants.ConfigurationsPath, name + Constants.JsonFileExtension);

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Configuration file for {name} not found at {filePath}");

        var configJsonStr = await File.ReadAllTextAsync(filePath);
        var config = JsonSerializer.Deserialize<GameConfiguration>(configJsonStr);

        if (config == null)
            throw new InvalidOperationException(
                $"Failed to deserialize the configuration for {name}. The JSON content might be invalid or empty."
            );

        return config;
    }

    public async Task SaveConfigAsync(GameConfiguration newConfig)
    {
        var filePath = Path.Combine(Constants.ConfigurationsPath, newConfig.Name + Constants.JsonFileExtension);
        var configJsonStr = JsonSerializer.Serialize(newConfig);
        await File.WriteAllTextAsync(filePath, configJsonStr);
    }

    public async Task DeleteConfigAsync(string name)
    {
        var filePath = Path.Combine(Constants.ConfigurationsPath, name + Constants.JsonFileExtension);

        if (File.Exists(filePath))
            await Task.Run(() => File.Delete(filePath));
        else
            throw new FileNotFoundException($"Configuration file for {name} not found at {filePath}");
    }

    private async Task CheckAndCreateInitialConfigAsync()
    {
        if (!Directory.Exists(Constants.ConfigurationsPath))
            Directory.CreateDirectory(Constants.ConfigurationsPath);

        var data = Directory.GetFiles(Constants.ConfigurationsPath, "*" + Constants.JsonFileExtension).ToList();
        if (data.Count != 0) return;

        var defaultGameConfigurations = GameConfiguration.GetDefaultGameConfigurations();
        foreach (var config in defaultGameConfigurations)
        {
            await SaveConfigAsync(config);
        }
    }
}
