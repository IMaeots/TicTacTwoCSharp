using Common.Entities;
using Data.Repositories;
using GameLogic;
using static Common.Constants;

namespace MenuSystem;

public abstract class BaseMenuSystem<TMenu>(
    IConfigRepository configRepository,
    IGameRepository gameRepository
) where TMenu : BaseMenu
{
    protected readonly IConfigRepository ConfigRepository = configRepository;
    protected readonly IGameRepository GameRepository = gameRepository;
    
    protected abstract string? StartNewGameWithConfig(string configName);
    protected abstract string? StartSavedGame(string savedGameName);
    protected abstract GameConfiguration CreateNewConfig();

    public void Run()
    {
        var result = CreateHomeMenu().Run();
        while (true)
        {
            switch (result)
            {
                case ReturnToMainShortcut:
                    result = CreateHomeMenu().Run();
                    break;
                case ExitShortcut:
                    return;
                case null:
                    return;
            }
        }
    }

    private static TMenu CreateMenu(EMenuLevel menuLevel, string menuHeader, string? menuDescription, List<MenuItem> menuItems) =>
        (TMenu)Activator.CreateInstance(typeof(TMenu), menuLevel, menuHeader, menuItems, menuDescription)!;
    
    private TMenu CreateHomeMenu()
    {
        var homeMenuItems = new List<MenuItem>
        {
            new (MenuNewGameTitle, MenuNewGameShortcut, CreateNewGameMenu().Run),
            new (MenuSavedGamesTitle, MenuSavedGamesShortcut, CreateSavedGamesMenu().Run), 
            new (MenuRulesAndInfoTitle, MenuRulesAndInfoShortcut, CreateRulesAndInfoMenu().Run)
        };

        return CreateMenu(EMenuLevel.Primary, GameName, null, homeMenuItems);
    }

    private TMenu CreateNewGameMenu()
    {
        var gameConfigurations = new List<MenuItem>
        {
            new(
                title: MenuConfigCreationTitle,
                shortcut: MenuConfigCreationShortcut,
                action: CreateNewConfigAndSaveIt
            )
        };

        var configNames = ConfigRepository.GetConfigurationNames();
        for (var i = 0; i < configNames.Count; i++)
        {
            var configIndex = i;
            gameConfigurations.Add(new MenuItem(
                title: configNames[i],
                shortcut: (i + 1).ToString(),
                action: () => StartNewGameWithConfig(configNames[configIndex])
            ));
        }

        return CreateMenu(EMenuLevel.Secondary, MenuChooseConfigHeading, null, gameConfigurations);
    }
    
    private string? CreateNewConfigAndSaveIt()
    {
        var newConfig = CreateNewConfig();
        ConfigRepository.SaveConfig(newConfig);
        return CreateNewGameMenu().Run();
    }
        
    private TMenu CreateSavedGamesMenu()
    {
        var savedGamesMenuItems = new List<MenuItem>();
        var savedGamesNames = GameRepository.GetSavedGamesNames();
        for (var i = 0; i < savedGamesNames.Count; i++)
        {
            var saveIndex = i;
            savedGamesMenuItems.Add(new MenuItem(
                title: savedGamesNames[i],
                shortcut: (i + 1).ToString(),
                action: () => StartSavedGame(savedGamesNames[saveIndex])
            ));
        }

        return CreateMenu(EMenuLevel.Secondary, MenuSavedGamesHeading, null, savedGamesMenuItems);
    }
    
    private TMenu CreateRulesAndInfoMenu()
    {
        var rulesMenuItems = new List<MenuItem>();
        return CreateMenu(EMenuLevel.Secondary, MenuRulesAndInfoHeading,
            MenuRulesAndInfoDescription, rulesMenuItems);
    }
}
