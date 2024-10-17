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
            
            if (menuItem.MenuItemAction != null)
            {
                return menuItem.RunAction();
            }

            switch (menuItem.Shortcut)
            {
                case Constants.ReturnShortcut:
                    return Constants.ReturnShortcut;
                
                case Constants.ExitShortcut:
                    if (ConfirmExit())
                    {
                        return Constants.ExitShortcut;
                    }
                    break;
                
                case Constants.ReturnToMainShortcut:
                    if (MenuLevel != EMenuLevel.Primary)
                    {
                        return Constants.ReturnToMainShortcut;
                    }
                    break;
            }
        } while (true);
    }

    private void DrawMenu()
    {
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
        Console.Write(Constants.MenuInputBoxHint);
    }

    private MenuItem DisplayMenuAndGetUserChoice()
    {
        do
        {
            DrawMenu();

            var userInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine(Constants.EmptyInputMessage);
            }
            else
            {
                var selectedMenuItem = MenuItems.FirstOrDefault(menuItem =>
                    menuItem.Shortcut.Equals(userInput, StringComparison.CurrentCultureIgnoreCase));
                
                if (selectedMenuItem != null)
                {
                    return selectedMenuItem;
                }

                Console.WriteLine(Constants.InvalidChoiceMessage + string.Join(", ", MenuItems.Select(mi => mi.Shortcut)));
            }

            Console.WriteLine();
        } while (true);
    }
    
    private static bool ConfirmExit()
    {
        Console.WriteLine(Constants.ConfirmExitText);
        var confirm = Console.ReadLine();
        Console.Clear();
        return confirm?.Trim().ToUpper() == "Y";
    }
}
