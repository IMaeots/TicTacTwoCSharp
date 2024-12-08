using System.Text.RegularExpressions;
using Common;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;
    
public class Info : PageModel
{
    public string HelpInfoForWeb { get; private set; } = string.Empty;
    

    public void OnGet()
    {
        HelpInfoForWeb = FormatForWeb(Constants.MenuRulesAndInfoDescription);
    }

    private string FormatForWeb(string input)
    {
        return Regex.Replace(input, @"\n", "<br>");
    }
}
