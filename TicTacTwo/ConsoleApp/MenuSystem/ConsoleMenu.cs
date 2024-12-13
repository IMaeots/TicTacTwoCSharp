using Common;
using Common.Entities;

namespace ConsoleApp.MenuSystem;

public class ConsoleMenu
{
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
        _menuItems = AddExitAndReturnMenuItems(menuLevel, menuItems);
        ValidateCorrectMenuInput(menuHeader, _menuItems);

        _menuLevel = menuLevel;
        _menuHeader = menuHeader;
        _menuDescription = menuDescription;
    }

    public string Run()
    {
        Console.Clear();
        do
        {
            var menuItem = DisplayMenuAndGetUserChoiceAsync().Result;

            var result = menuItem.Shortcut;
            if (menuItem.MenuItemAction != null) result = menuItem.RunAction();

            switch (result)
            {
                case Constants.ExitShortcut:
                    if (ConfirmExit(Constants.ConfirmExitText)) return Constants.ManualExitShortcut;
                    break;
                case Constants.ReturnShortcut:
                    if (_menuLevel != EMenuLevel.Primary) return Constants.ReturnToMainShortcut;
                    break;
                case Constants.ReturnToMainShortcut:
                    if (_menuLevel != EMenuLevel.Primary) return Constants.ReturnToMainShortcut;
                    break;
                case Constants.ManualExitShortcut:
                    return Constants.ManualExitShortcut;
                case Constants.LeaveGameShortcut:
                    return Constants.LeaveGameShortcut;
            }
        } while (true);
    }

    private static void ValidateCorrectMenuInput(string menuHeader, List<MenuItem> menuItems)
    {
        if (string.IsNullOrWhiteSpace(menuHeader)) throw new ApplicationException("Menu header cannot be empty.");

        var menuItemShortCuts = new List<string>();
        foreach (var item in menuItems)
        {
            if (menuItemShortCuts.Contains(item.Shortcut))
                throw new ApplicationException("There are duplicate menu shortcuts.");

            menuItemShortCuts.Add(item.Shortcut);
        }
    }

    private static List<MenuItem> AddExitAndReturnMenuItems(EMenuLevel menuLevel, List<MenuItem> menuItems)
    {
        var newMenuItems = new List<MenuItem>(menuItems) { new(Constants.ExitTitle, Constants.ExitShortcut) };

        if (menuLevel != EMenuLevel.Primary)
            newMenuItems.Add(new MenuItem(Constants.ReturnTitle, Constants.ReturnShortcut));

        if (menuLevel == EMenuLevel.Tertiary)
            newMenuItems.Add(new MenuItem(Constants.ReturnToMainTitle, Constants.ReturnToMainShortcut));

        return newMenuItems;
    }

    private Task<MenuItem> DisplayMenuAndGetUserChoiceAsync()
    {
        string? errorMessage = null;
        do
        {
            DrawMenu();

            if (errorMessage != null) Console.WriteLine(errorMessage);

            Console.Write(Constants.MenuInputBoxHint);
            var userInput = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                errorMessage = Constants.EmptyInputMessage;
            }
            else
            {
                var selectedMenuItem = _menuItems.FirstOrDefault(menuItem =>
                    menuItem.Shortcut.Equals(userInput, StringComparison.CurrentCultureIgnoreCase));

                if (selectedMenuItem != null) return Task.FromResult(selectedMenuItem);

                errorMessage = Constants.InvalidChoiceMessage + string.Join(", ", _menuItems.Select(mi => mi.Shortcut));
            }
        } while (true);
    }

    private void DrawMenu()
    {
        Console.Clear();
        Console.WriteLine(_menuHeader);
        if (_menuDescription != null)
        {
            Console.WriteLine(Constants.MenuDescriptionDivider);
            Console.WriteLine(_menuDescription);
        }

        Console.WriteLine(Constants.MenuDivider);
        foreach (var menuItem in _menuItems)
        {
            Console.WriteLine(menuItem);
        }

        Console.WriteLine();
    }

    public static bool ConfirmExit(string message)
    {
        Console.WriteLine(message);
        var confirm = Console.ReadLine();
        Console.Clear();
        return confirm?.Trim().ToUpper() == Constants.ConfirmSymbol;
    }
}
