using GameBrain;

namespace Data;

public interface IConfigRepository
{
    List<string> GetConfigurationNames();
    GameConfiguration? GetConfigurationByName(string name);
    void SaveConfig(GameConfiguration? newConfig);
}
