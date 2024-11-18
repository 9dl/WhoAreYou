namespace WhoAreYou.Helpers;

public static class Interface
{
    public static object ShowOptions(Delegate function)
    {
        var methodInfo = function.Method;
        var parameters = methodInfo.GetParameters();
        var parameterValues = new object[parameters.Length];

        for (var i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];
            var defaultValue = parameter.HasDefaultValue
                ? parameter.DefaultValue
                : GetDefaultValueForType(parameter.ParameterType);

            Console.Clear();
            Print("Info", "Press Enter to use the default value", "?");
            Print($"{parameter.Name}", $"Default value: {defaultValue}", parameter.ParameterType.Name);

            var inputValue = GetInputForParameter(parameter.ParameterType, defaultValue);
            parameterValues[i] = inputValue;
        }

        var result = function.DynamicInvoke(parameterValues);
        return result;
    }

    private static object GetInputForParameter(Type paramType, object defaultValue)
    {
        if (paramType == typeof(bool))
        {
            Read("true/false");
            var input = Console.ReadLine()?.Trim().ToLower();
            return string.IsNullOrEmpty(input) ? defaultValue : bool.Parse(input);
        }

        if (paramType == typeof(int))
        {
            Read("integer");
            var input = Console.ReadLine()?.Trim();
            return string.IsNullOrEmpty(input) ? defaultValue : int.Parse(input);
        }

        if (paramType == typeof(string))
        {
            Read("string");
            var input = Console.ReadLine()?.Trim();
            return string.IsNullOrEmpty(input) ? defaultValue : input;
        }

        return defaultValue;
    }

    private static object GetDefaultValueForType(Type paramType)
    {
        if (paramType == typeof(bool)) return false;
        if (paramType == typeof(int)) return 0;
        if (paramType == typeof(string)) return string.Empty;
        return null;
    }

    private static void Print(string option, string value, string unit)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("[");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(option);

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("] ");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("[");

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write(unit);

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("] ");

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(value);
    }

    private static void Read(string unit)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("[");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(unit);

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("] ");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("[");

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write(">");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("] ");

        Console.ForegroundColor = ConsoleColor.Gray;
    }

    public static void PrintLine(string option, string value, bool newLine = true)
    {
        Dictionary<string, ConsoleColor> options = new()
        {
            { "!", ConsoleColor.Red },
            { "?", ConsoleColor.Cyan },
            { "~", ConsoleColor.Yellow },
            { ">", ConsoleColor.Magenta }
        };

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("[");

        Console.ForegroundColor = options.ContainsKey(option) || int.TryParse(option, out _)
            ? options.ContainsKey(option) ? options[option] : ConsoleColor.Green
            : ConsoleColor.Cyan;

        Console.Write(option);

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("] ");

        Console.ForegroundColor = ConsoleColor.Gray;
        if (newLine)
            Console.WriteLine(value);
        else
            Console.Write(value);
    }

    public static string ReadLine()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("[");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(">");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("] ");

        Console.ForegroundColor = ConsoleColor.Gray;
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }
}