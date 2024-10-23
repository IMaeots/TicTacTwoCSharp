using Common;
using Common.Entities;

namespace MenuSystem;

public abstract class BaseMenu
{
    protected readonly EMenuLevel MenuLevel;
    protected readonly string MenuHeader;
    protected readonly string? MenuDescription;
    protected readonly List<MenuItem> MenuItems;

    protected BaseMenu(
        EMenuLevel menuLevel,
        string menuHeader,
        List<MenuItem> menuItems,
        string? menuDescription = null
    )
    {
        MenuItems = AddExitAndReturnMenuItems(menuLevel, menuItems);
        ValidateCorrectMenuInput(menuHeader, MenuItems);
        
        MenuLevel = menuLevel;
        MenuHeader = menuHeader;
        MenuDescription = menuDescription;
    }

    public abstract string? Run();

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

    private List<MenuItem> AddExitAndReturnMenuItems(EMenuLevel menuLevel, List<MenuItem> menuItems)
    {
        var newMenuItems = new List<MenuItem>(menuItems)
        {
            new(Constants.ExitTitle, Constants.ExitShortcut)
        };

        if (menuLevel != EMenuLevel.Primary)
        {
            newMenuItems.Add(new MenuItem(Constants.ReturnTitle, Constants.ReturnShortcut));
        }

        if (menuLevel == EMenuLevel.Tertiary)
        {
            newMenuItems.Add(new MenuItem(Constants.ReturnToMainTitle, Constants.ReturnToMainShortcut));
        }

        return newMenuItems;
    }
}
