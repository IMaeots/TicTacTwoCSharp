using GameBrain;

namespace Data.Repositories;

public class ConfigRepositoryJson : IConfigRepository
{
    public List<string> GetConfigurationNames()
    {
        throw new NotImplementedException();
    }

    public GameConfiguration? GetConfigurationByName(string name)
    {
        throw new NotImplementedException();
    }

    public void SaveConfig(GameConfiguration? newConfig)
    {
        throw new NotImplementedException();
    }
}
