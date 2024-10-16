using Common.Entities;
using Data;
using Data.Repositories;
using MenuSystem;

namespace ConsoleApp;

public static class Menus
{
    private static readonly IConfigRepository ConfigRepository = new ConfigRepositoryHardcoded(); // TODO: Switch out for the dynamic one later.
    
    public static readonly Menu RulesMenu = new(
        menuLevel: EMenuLevel.Secondary,
        menuHeader: "TIC-TAC-TOE Rules & Restrictions",
        menuItems: [],
        menuDescription: "Rule1: sadsad \n Rule2: sadsad \n Rule3: sadsad "
    );
    public static readonly Menu OptionsMenu = new(
        menuLevel: EMenuLevel.Secondary,
        menuHeader: "TIC-TAC-TOE Options",
        menuItems: [
            new MenuItem("X Starts", "X", DummyMethod),
            new MenuItem("O Starts", "O", DummyMethod)
        ]
    );
    public static readonly Menu HomeMenu = new(
        menuLevel: EMenuLevel.Primary,
        menuHeader: "TIC-TAC-TWO",
        menuItems: [
            new MenuItem("New Game", "N", ShowConfigMenu),
            new MenuItem("Options", "O", OptionsMenu.Run),
            new MenuItem("Rules", "R", RulesMenu.Run)
        ]
    );

    private static string DummyMethod()
    {
        Console.Write("DummyMethod");
        Console.ReadKey();
        return "foobar";
    }
    
    private static string ShowConfigMenu()
    {
        var configMenuItems = new List<MenuItem>();
        var configNames = ConfigRepository.GetConfigurationNames();
        
        for (var i = 0; i < configNames.Count; i++)
        {
            var configIndex = i;
            configMenuItems.Add(
                new MenuItem(
                    title: configNames[i],
                    shortcut: (i + 1).ToString(),
                    action: () => StartGameWithConfig(configNames[configIndex])
                )
            );
        }

        var configMenu = new Menu(
            menuLevel: EMenuLevel.Secondary,
            menuHeader: "TIC-TAC-TWO - Choose Game Configuration",
            menuItems: configMenuItems
        );

        return configMenu.Run();
    }
    
    private static string StartGameWithConfig(string configName)
    {
        var config = ConfigRepository.GetConfigurationByName(configName);
        if (config == null)
        {
            Console.WriteLine("Invalid configuration selected.");
            return "Error";
        }
        
        GameController.StartGame(config);
        return "Game started";
    }
}
