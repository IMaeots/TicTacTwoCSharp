using Common;
using Common.Entities;

namespace ConsoleApp.MenuSystem;

public class ConsoleMenu
{
    private const string MenuDescriptionDivider = "---------------";
    private const string MenuDivider = "===============";
    private const string MenuInputBoxHint = ">";
    private const string ExitTitle = "Exit";
    private const string ReturnTitle = "Return";
    private const string ReturnToMainTitle = "Return to Main menu";
    private const string EmptyInputMessage = "It would be nice, if you actually choose something!!! Try again... Maybe...";
    private const string InvalidChoiceMessage = "Invalid choice. Available options: ";
    
    private readonly EMenuLevel _menuLevel;
    private readonly string _menuHeader;
    private readonly string? _menuDescription;
    private readonly List<MenuItem> _menuItems;

    public ConsoleMenu(
        EMenuLevel menuLevel,
        string menuHeader,
        List<MenuItem> menuItems,
        string? menuDescription = null
    )
    {
        _menuItems = AddStandardMenuItems(menuLevel, menuItems);
        ValidateMenuConfiguration(menuHeader, _menuItems);

        _menuLevel = menuLevel;
        _menuHeader = menuHeader;
        _menuDescription = menuDescription;
    }

    public string Run()
    {
        Console.Clear();
        
        while (true)
        {
            var menuItem = GetUserMenuSelection();
            var result = ExecuteMenuAction(menuItem);
            
            if (ShouldExitMenu(result))
            {
                return result;
            }
        }
    }

    private bool ShouldExitMenu(string result)
    {
        switch (result)
        {
            case Constants.ExitShortcut:
                return true;
            case Constants.ReturnShortcut:
                return _menuLevel != EMenuLevel.Primary;
            case Constants.ReturnToMainShortcut:
            case Constants.ManualExitShortcut:
            case Constants.LeaveGameShortcut:
                return true;
        }
        
        return false;
    }

    private string ExecuteMenuAction(MenuItem menuItem)
    {
        return menuItem.MenuItemAction != null 
            ? menuItem.RunAction() ?? Constants.ReturnToMainShortcut 
            : menuItem.Shortcut;
    }

    private MenuItem GetUserMenuSelection()
    {
        string? errorMessage = null;
        
        while (true)
        {
            DrawMenu(errorMessage);

            var userInput = ReadUserInput();
            
            if (string.IsNullOrWhiteSpace(userInput))
            {
                errorMessage = EmptyInputMessage;
                continue;
            }
            
            var selectedMenuItem = _menuItems.FirstOrDefault(menuItem =>
                menuItem.Shortcut.Equals(userInput, StringComparison.CurrentCultureIgnoreCase));

            if (selectedMenuItem != null)
            {
                return selectedMenuItem;
            }
            
            errorMessage = $"{InvalidChoiceMessage}{GetAvailableOptions()}";
        }
    }

    private string GetAvailableOptions()
    {
        return string.Join(", ", _menuItems.Select(mi => mi.Shortcut));
    }

    private string ReadUserInput()
    {
        Console.Write(MenuInputBoxHint);
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    private void DrawMenu(string? errorMessage = null)
    {
        Console.Clear();
        Console.WriteLine(_menuHeader);
        
        if (_menuDescription != null)
        {
            Console.WriteLine(MenuDescriptionDivider);
            Console.WriteLine(_menuDescription);
        }

        Console.WriteLine(MenuDivider);
        
        foreach (var menuItem in _menuItems)
        {
            Console.WriteLine(menuItem);
        }

        Console.WriteLine();
        
        if (errorMessage != null)
        {
            Console.WriteLine(errorMessage);
        }
    }

    private static void ValidateMenuConfiguration(string menuHeader, List<MenuItem> menuItems)
    {
        if (string.IsNullOrWhiteSpace(menuHeader)) 
        {
            throw new ArgumentException("Menu header cannot be empty.");
        }

        var shortcuts = menuItems.Select(item => item.Shortcut).ToList();
        var duplicateShortcuts = shortcuts.GroupBy(s => s)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();
        
        if (duplicateShortcuts.Count != 0)
        {
            throw new ArgumentException($"Duplicate menu shortcuts found: {string.Join(", ", duplicateShortcuts)}");
        }
    }

    private static List<MenuItem> AddStandardMenuItems(EMenuLevel menuLevel, List<MenuItem> menuItems)
    {
        var result = new List<MenuItem>(menuItems);
        
        result.Add(new MenuItem(ExitTitle, Constants.ExitShortcut));

        if (menuLevel != EMenuLevel.Primary)
        {
            result.Add(new MenuItem(ReturnTitle, Constants.ReturnShortcut));
        }

        if (menuLevel == EMenuLevel.Tertiary)
        {
            result.Add(new MenuItem(ReturnToMainTitle, Constants.ReturnToMainShortcut));
        }

        return result;
    }
}
