namespace Data.Models.file;

public class JsonGameModel
{
    public string Name { get; set; } = default!;
    public string JsonConfiguration { get; set; } = default!;
    public List<string> JsonGameStates { get; set; } = [];
    public string? PasswordP1 { get; set; }
    public string? PasswordP2 { get; set; }
}
