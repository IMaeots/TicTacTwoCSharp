namespace ConsoleApp.MenuSystem;

public class MenuItem
{
    private string Title { get; }
    public string Shortcut { get; }
    public Func<string?>? MenuItemAction { get; }
    public Func<Task<string?>>? AsyncMenuItemAction { get; }

    public MenuItem(string title, string shortcut, Func<string?>? action = null)
    {
        ValidateMenuItemInitializationInput(title, shortcut);

        Title = title;
        Shortcut = shortcut;
        MenuItemAction = action;
    }

    public MenuItem(string title, string shortcut, Func<Task<string?>>? asyncAction)
    {
        ValidateMenuItemInitializationInput(title, shortcut);

        Title = title;
        Shortcut = shortcut;
        AsyncMenuItemAction = asyncAction;
    }

    private void ValidateMenuItemInitializationInput(string title, string shortcut)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty");

        if (string.IsNullOrWhiteSpace(shortcut))
            throw new ArgumentException("Shortcut cannot be empty");
    }

    public async Task<string?> RunAction()
    {
        if (MenuItemAction != null) return MenuItemAction();
        if (AsyncMenuItemAction != null) return await AsyncMenuItemAction();
        throw new InvalidOperationException("No action assigned to this menu item.");
    }

    public override string ToString() => $"{Shortcut}) {Title}";
}
