namespace MenuSystem;

public class MenuItem
{
    private string Title { get; }
    public string Shortcut { get; }
    public Func<string?>? MenuItemAction { get; set; }
    
    public MenuItem(string title, string shortcut, Func<string?>? action = null)
    {
        ValidateMenuItemInitializationInput(title, shortcut);
        
        Title = title;
        Shortcut = shortcut;
        MenuItemAction = action;
    }

    private void ValidateMenuItemInitializationInput(string title, string shortcut)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be empty");
        }
        
        if (string.IsNullOrWhiteSpace(shortcut))
        {
            throw new ArgumentException("Shortcut cannot be empty");
        }
    }

    public string? RunAction()
    {
        if (MenuItemAction != null)
        {
            return MenuItemAction();
        }
        throw new InvalidOperationException("No action assigned to this menu item.");
    }

    public override string ToString()
    {
        return $"{Shortcut}) {Title}";
    }
}
