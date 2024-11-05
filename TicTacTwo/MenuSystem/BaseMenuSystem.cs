using Common.Entities;
using Data;
using Data.Repositories;
using GameBrain;
using static Common.Constants;

namespace MenuSystem;

public abstract class BaseMenuSystem<TMenu> where TMenu : BaseMenu
{
    protected readonly IConfigRepository ConfigRepository;
    protected readonly IGameRepository GameRepository;
    
    private readonly TMenu _homeMenu;
    private readonly TMenu _rulesMenu;
    
    protected abstract string? StartGameWithConfig(string configName);
    protected abstract string? StartSavedGame(string savedGameName);
    protected abstract GameConfiguration CreateNewConfig();

    protected BaseMenuSystem(IConfigRepository configRepository, IGameRepository gameRepository)
    {
        ConfigRepository = configRepository;
        GameRepository = gameRepository;
        
        _rulesMenu = CreateInfoMenu();
        _homeMenu = CreateHomeMenu();
    }

    public void Run()
    {
        var result = _homeMenu.Run();
        while (true)
        {
            switch (result)
            {
                case ReturnToMainShortcut:
                    result = _homeMenu.Run();
                    break;
                case ExitShortcut:
                    return;
                case null:
                    return;
            }
        }
    }
    
    private TMenu CreateMenu(EMenuLevel menuLevel, string menuHeader, string? menuDescription, List<MenuItem> menuItems)
    {
        return (TMenu)Activator.CreateInstance(typeof(TMenu), menuLevel, menuHeader, menuItems, menuDescription)!;
    }
    
    private TMenu CreateHomeMenu()
    {
        var homeMenuItems = new List<MenuItem>
        {
            new (MenuNewGameTitle, MenuNewGameShortcut, CreateConfigMenuAndRunIt),
            new (MenuSavedGamesTitle, MenuSavedGamesShortcut, CreateSavedGamesMenuAndRunIt), 
            new (MenuRulesAndInfoTitle, MenuRulesAndInfoShortcut, _rulesMenu.Run)
        };

        return CreateMenu(EMenuLevel.Primary, GameName, null, homeMenuItems);
    }

    private TMenu CreateInfoMenu()
    {
        var rulesMenuItems = new List<MenuItem>();
        return CreateMenu(EMenuLevel.Secondary, MenuRulesAndInfoHeading,
            MenuRulesAndInfoDescription, rulesMenuItems);
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
    
    private string? CreateSavedGamesMenuAndRunIt()
    {
        var savedGamesMenu = CreateSavedGamesMenu();
        return savedGamesMenu.Run();
    }

    private TMenu CreateConfigMenu()
    {
        var configMenuItems = new List<MenuItem>();
        var configNames = ConfigRepository.GetConfigurationNames();

        configMenuItems.Add(new MenuItem(
            title: MenuConfigCreationTitle,
            shortcut: MenuConfigCreationShortcut,
            action: CreateNewConfigAndRunIt
        ));
        
        for (var i = 0; i < configNames.Count; i++)
        {
            var configIndex = i;
            configMenuItems.Add(new MenuItem(
                title: configNames[i],
                shortcut: (i + 1).ToString(),
                action: () => StartGameWithConfig(configNames[configIndex])
            ));
        }

        return CreateMenu(EMenuLevel.Secondary, MenuChooseConfigHeading, null, configMenuItems);
    }

    private string? CreateConfigMenuAndRunIt()
    {
        var configMenu = CreateConfigMenu();
        return configMenu.Run();
    }

    private string? CreateNewConfigAndRunIt()
    {
        var newConfig = CreateNewConfig();
        ConfigRepository.SaveConfig(newConfig);
        return StartGameWithConfig(newConfig.Name);
    }
}
