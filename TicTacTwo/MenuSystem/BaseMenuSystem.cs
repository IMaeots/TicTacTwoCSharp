using Common;
using Common.Entities;
using Data;
using GameBrain;

namespace MenuSystem;

public abstract class BaseMenuSystem<TMenu> where TMenu : BaseMenu
{
    protected readonly IConfigRepository ConfigRepository;
    
    private readonly TMenu _homeMenu;
    private readonly TMenu _savedGamesMenu;
    private readonly TMenu _optionsMenu;
    private readonly TMenu _rulesMenu;

    protected BaseMenuSystem(IConfigRepository configRepository)
    {
        ConfigRepository = configRepository;
        
        _rulesMenu = CreateInfoMenu();
        _optionsMenu = CreateOptionsMenu();
        _savedGamesMenu = CreateSavedGamesMenu();
        _homeMenu = CreateHomeMenu();
    }

    protected abstract string? StartGameWithConfig(string configName);
    protected abstract GameConfiguration CreateNewConfig();

    public void Run()
    {
        while (true)
        {
            var result = _homeMenu.Run();
            
            switch (result)
            {
                case Constants.ReturnToMainTitle:
                    _homeMenu.Run();
                    break;
                case Constants.ExitShortcut:
                    return;
                case null:
                    return;
            }
        }
    }

    private TMenu CreateInfoMenu()
    {
        var rulesMenuItems = new List<MenuItem>();
        return CreateMenu(EMenuLevel.Secondary, Constants.InfoMenuTitle,
            Constants.InfoMenuDescription, rulesMenuItems);
    }

    private TMenu CreateOptionsMenu()
    {
        var optionsMenuItems = new List<MenuItem>();
        return CreateMenu(EMenuLevel.Secondary, Constants.OptionsMenuTitle, null, optionsMenuItems);
    }
        
    private TMenu CreateSavedGamesMenu()
    {
        // TODO: List saved games: Clicking on them starts the game from where it left of.
        var savedGamesMenuItems = new List<MenuItem>();
        return CreateMenu(EMenuLevel.Secondary, Constants.SavedGamesMenuTitle, null, savedGamesMenuItems);
    }

    private TMenu CreateHomeMenu()
    {
        var homeMenuItems = new List<MenuItem>
        {
            new ("New Game", "N", CreateConfigMenuAndRunIt),
            new ("Saved Games", "S", _savedGamesMenu.Run), 
            new ("Options", "O", _optionsMenu.Run),
            new ("Rules", "R", _rulesMenu.Run)
        };

        return CreateMenu(EMenuLevel.Primary, Constants.GameName, null, homeMenuItems);
    }

    private TMenu CreateConfigMenu()
    {
        var configMenuItems = new List<MenuItem>();
        var configNames = ConfigRepository.GetConfigurationNames();

        configMenuItems.Add(new MenuItem(
            title: "Create New Config",
            shortcut: "C",
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

        return CreateMenu(EMenuLevel.Secondary, Constants.ChooseConfigMenuTitle, null, configMenuItems);
    }

    private TMenu CreateMenu(EMenuLevel menuLevel, string menuHeader, string? menuDescription, List<MenuItem> menuItems)
    {
        return (TMenu)Activator.CreateInstance(typeof(TMenu), menuLevel, menuHeader, menuItems, menuDescription)!;
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
