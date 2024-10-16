using Common.Entities;
using Constants = Common.Constants;

namespace MenuSystem;

public class Menu
{
    private EMenuLevel MenuLevel { get; set; }
    private string MenuHeader { get; set; }
    private string? MenuDescription { get; set; }
    private List<MenuItem> MenuItems { get; set; }

    public Menu(
        EMenuLevel menuLevel,
        string menuHeader,
        List<MenuItem> menuItems,
        string? menuDescription = null
    )
    {
        menuItems.AddRange(AddExitAndReturnMenuItems(menuLevel));
        
        ValidateCorrectMenuInput(menuHeader, menuItems);
        
        MenuLevel = menuLevel;
        MenuHeader = menuHeader;
        MenuItems = [..menuItems];
        MenuDescription = menuDescription;
    }
    
    public string Run()
    {
        Console.Clear();
        do
        {
            var menuItem = DisplayMenuGetUserChoice();
            
            if (menuItem.MenuItemAction != null)
            {
                return menuItem.MenuItemAction();
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

    private void ValidateCorrectMenuInput(string menuHeader, List<MenuItem> menuItems)
    {
        if (string.IsNullOrWhiteSpace(menuHeader))
        {
            throw new ApplicationException("Menu header cannot be empty.");
        }

        var menuItemShortCuts = new List<string>();
        foreach (var item in menuItems)
        {
            if (menuItemShortCuts.Contains(item.Shortcut))
            {
                throw new ApplicationException("There are duplicate menu shortcuts.");
            }
        
            menuItemShortCuts.Add(item.Shortcut);
        }
    }

    private List<MenuItem> AddExitAndReturnMenuItems(EMenuLevel menuLevel)
    {
        var menuItems = new List<MenuItem> { new(Constants.ExitTitle, Constants.ExitShortcut) };

        if (menuLevel != EMenuLevel.Primary)
        {
            menuItems.Add(new MenuItem(Constants.ReturnTitle, Constants.ReturnShortcut));
        }

        if (menuLevel == EMenuLevel.Tertiary)
        {
            menuItems.Add(new MenuItem(Constants.ReturnToMainTitle, Constants.ReturnToMainShortcut));
        }

        return menuItems;
    }

    public void SetMenuItemAction(string shortCut, Func<string> action)
    {
        var menuItem = MenuItems.SingleOrDefault(m => m.Shortcut.Equals(shortCut, StringComparison.OrdinalIgnoreCase));
        if (menuItem != null)
        {
            menuItem.MenuItemAction = action;
        }
        else
        {
            throw new ApplicationException($"No menu item found with shortcut: {shortCut}");
        }
    }

    private bool ConfirmExit()
    {
        Console.WriteLine("Are you sure you want to exit? (Y/N)");
        var confirm = Console.ReadLine();
        return confirm?.Trim().ToUpper() == "Y";
    }


    private MenuItem DisplayMenuGetUserChoice()
    {
        do
        {
            DrawMenu();
            
            var userInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine("It would be nice, if you actually choose something!!! Try again... Maybe..."); // Constants.EmptyInputMessage
            }
            else
            {
                userInput = userInput.ToUpper();
                foreach (var menuItem in MenuItems.Where(menuItem => menuItem.Shortcut.ToUpper() == userInput))
                {
                    return menuItem;
                }

                Console.WriteLine("Invalid choice. Available options: " + string.Join(", ", MenuItems.Select(mi => mi.Shortcut)));
            }

            Console.WriteLine();
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
}
