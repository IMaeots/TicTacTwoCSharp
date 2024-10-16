namespace MenuSystem;

public class MenuItem
{
    private readonly string _title = default!;
    private readonly string _shortcut = default!;
    
    public MenuItem(string title, string shortcut, Func<string>? action = null)
    {
        Title = title;
        Shortcut = shortcut;
        MenuItemAction = action;
    }

    public Func<string>? MenuItemAction { get; set; }

    private string Title
    {
        get => _title;
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Title cannot be empty");
            }
            _title = value;
        } 
    }

    public string Shortcut
    {
        get => _shortcut;
        private init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Shortcut cannot be empty");
            }
            _shortcut = value;
        }
    }

    public string RunAction()
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
