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
        await EnsureDirectoriesExistAsync();
        
        var filePath = GetConfigurationFilePath(name);

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Configuration file for {name} not found at {filePath}");

        try
        {
            var configJsonStr = await File.ReadAllTextAsync(filePath);
            var config = JsonSerializer.Deserialize<GameConfiguration>(configJsonStr);

            if (config == null)
                throw new InvalidOperationException(
                    $"Failed to deserialize the configuration for {name}. The JSON content might be invalid or empty."
                );

            return config;
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Error parsing configuration data for {name}: {ex.Message}", ex);
        }
        catch (Exception ex) when (ex is not FileNotFoundException and not InvalidOperationException)
        {
            throw new InvalidOperationException($"Unexpected error loading configuration {name}: {ex.Message}", ex);
        }
    }

    public async Task SaveConfigAsync(GameConfiguration newConfig)
    {
        await EnsureDirectoriesExistAsync();
        
        var filePath = GetConfigurationFilePath(newConfig.Name);
        
        try
        {
            var configJsonStr = JsonSerializer.Serialize(newConfig);
            await File.WriteAllTextAsync(filePath, configJsonStr);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to save configuration '{newConfig.Name}': {ex.Message}", ex);
        }
    }

    public async Task DeleteConfigAsync(string name)
    {
        await EnsureDirectoriesExistAsync();
        
        var filePath = GetConfigurationFilePath(name);

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Configuration file for {name} not found at {filePath}");
            
        try
        {
            await Task.Run(() => File.Delete(filePath));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to delete configuration '{name}': {ex.Message}", ex);
        }
    }

    private async Task CheckAndCreateInitialConfigAsync()
    {
        await EnsureDirectoriesExistAsync();

        var data = Directory.GetFiles(Constants.ConfigurationsPath, "*" + Constants.JsonFileExtension).ToList();
        if (data.Count != 0) return;

        var defaultGameConfigurations = GameConfiguration.GetDefaultGameConfigurations();
        foreach (var config in defaultGameConfigurations)
        {
            await SaveConfigAsync(config);
        }
    }
    
    private static string GetConfigurationFilePath(string configName) =>
        Path.Combine(Constants.ConfigurationsPath, configName + Constants.JsonFileExtension);
        
    private static async Task EnsureDirectoriesExistAsync()
    {
        await Task.Run(() => {
            if (!Directory.Exists(Constants.ConfigurationsPath))
                Directory.CreateDirectory(Constants.ConfigurationsPath);
            
            if (!Directory.Exists(Constants.GamesPath))
                Directory.CreateDirectory(Constants.GamesPath);
        });
    }
}
