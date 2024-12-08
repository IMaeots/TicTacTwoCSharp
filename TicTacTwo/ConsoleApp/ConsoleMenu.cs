using Common;
using Common.Entities;
using MenuSystem;

namespace ConsoleApp;

// ReSharper disable once ClassNeverInstantiated.Global
public class ConsoleMenu(
    EMenuLevel menuLevel,
    string menuHeader,
    List<MenuItem> menuItems,
    string? menuDescription = null
) : BaseMenu(menuLevel, menuHeader, menuItems, menuDescription)
{
    public override string? Run()
    {
        Console.Clear();
        do
        {
            var menuItem = DisplayMenuAndGetUserChoice();

            if (menuItem.MenuItemAction != null) return menuItem.RunAction();

            switch (menuItem.Shortcut)
            {
                case Constants.ExitShortcut:
                    if (ConfirmExit(Constants.ConfirmExitText)) return Constants.ExitShortcut;
                    break;
                case Constants.LeaveGameShortcut:
                    if (ConfirmExit(Constants.ConfirmLeaveGameText)) return Constants.ReturnToMainShortcut;
                    break;
                case Constants.ReturnShortcut:
                    if (MenuLevel != EMenuLevel.Primary) return Constants.ReturnToMainShortcut;
                    break;
                case Constants.ReturnToMainShortcut:
                    if (MenuLevel != EMenuLevel.Primary) return Constants.ReturnToMainShortcut;
                    break;
            }
        } while (true);
    }

    private MenuItem DisplayMenuAndGetUserChoice()
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
                var selectedMenuItem = MenuItems.FirstOrDefault(menuItem =>
                    menuItem.Shortcut.Equals(userInput, StringComparison.CurrentCultureIgnoreCase));

                if (selectedMenuItem != null) return selectedMenuItem;

                errorMessage = Constants.InvalidChoiceMessage + string.Join(", ", MenuItems.Select(mi => mi.Shortcut));
            }
        } while (true);
    }

    private void DrawMenu()
    {
        Console.Clear();
        Console.WriteLine(MenuHeader);
        if (MenuDescription != null)
        {
            Console.WriteLine(Constants.MenuDescriptionDivider);
            Console.WriteLine(MenuDescription);
        }

        Console.WriteLine(Constants.MenuDivider);
        foreach (var menuItem in MenuItems)
        {
            Console.WriteLine(menuItem);
        }

        Console.WriteLine();
    }

    private static bool ConfirmExit(string message)
    {
        Console.WriteLine(message);
        var confirm = Console.ReadLine();
        Console.Clear();
        return confirm?.Trim().ToUpper() == Constants.ConfirmSymbol;
    }
}
