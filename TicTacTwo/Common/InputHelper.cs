namespace Common;

public static class InputHelper
{
    public static string GetValidatedName(string prompt, List<string> existingNames,
        Func<string, string?> validationRule)
    {
        string input;
        string? errorMessage;
        do
        {
            Console.WriteLine(prompt);
            input = Console.ReadLine() ?? string.Empty;
            errorMessage = string.IsNullOrWhiteSpace(input)
                ? "Input cannot be empty or whitespace."
                : existingNames.Contains(input)
                    ? "Name already exists."
                    : validationRule(input);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                Console.WriteLine(errorMessage);
            }
        } while (!string.IsNullOrEmpty(errorMessage));

        return input;
    }

    public static string GetValidatedString(string prompt, Func<string, string?> validationRule)
    {
        string input;
        string? errorMessage;
        do
        {
            Console.WriteLine(prompt);
            input = Console.ReadLine() ?? string.Empty;
            errorMessage = string.IsNullOrWhiteSpace(input)
                ? "Input cannot be empty or whitespace."
                : validationRule(input);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                Console.WriteLine(errorMessage);
            }
        } while (!string.IsNullOrEmpty(errorMessage));

        return input;
    }

    public static int GetValidatedInt(string prompt, Func<int, string?> validationRule)
    {
        int value;
        string? errorMessage;
        do
        {
            Console.WriteLine(prompt);
            var input = Console.ReadLine() ?? string.Empty;
            var isInputInt = int.TryParse(input, out value);
            errorMessage = isInputInt ? validationRule(value) : "Input must be an integer.";

            if (errorMessage != null)
            {
                Console.WriteLine(errorMessage);
            }
        } while (errorMessage != null);

        return value;
    }

    public static int? GetValidatedNullableInt(string prompt, Func<int, string?> validationRule)
    {
        int? value;
        string? errorMessage;
        do
        {
            value = null;
            Console.WriteLine(prompt);
            var input = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(input))
            {
                errorMessage = null;
            }
            else
            {
                var isInputInt = int.TryParse(input, out var parsedValue);
                value = parsedValue;
                errorMessage = isInputInt ? validationRule(parsedValue) : "Input must be an integer.";
            }

            if (errorMessage != null)
            {
                Console.WriteLine(errorMessage);
            }
        } while (errorMessage != null);

        return value;
    }
}
