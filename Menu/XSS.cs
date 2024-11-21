using WhoAreYou.Helpers;
using WhoAreYou.Modules;
using File = WhoAreYou.Helpers.File;

namespace WhoAreYou.Menu;

public static class XssMenu
{
    public static async Task Menu()
    {
        Console.Clear();
        Interface.PrintLine("XSS", "Cross-Site Scripting");
        Interface.PrintLine("?", "Choose an option:");
        Interface.PrintLine("1", "Generate Payloads");
        Interface.PrintLine("2", "Scan for XSS");
        Interface.PrintLine("0", "Exit");

        var choice = Interface.ReadLine();

        switch (choice)
        {
            case "1":
                PayloadsGenerator();
                break;
            case "2":
                await XssScanner();
                break;
            default:
                Interface.PrintLine("!", "Invalid choice. Please try again.");
                Interface.ReadLine();
                break;
        }
    }

    private static async Task XssScanner()
    {
        try
        {
            Console.Clear();
            Interface.PrintLine("?", "Type URL");
            var url = Interface.ReadLine();

            if (string.IsNullOrEmpty(url))
            {
                Interface.PrintLine("~", "URL cannot be empty.");
                return;
            }

            Console.Clear();
            Interface.PrintLine("?", "Type the XSS payload");
            var xssPayload = Interface.ReadLine();

            if (string.IsNullOrEmpty(xssPayload))
            {
                Interface.PrintLine("~", "XSS payload cannot be empty.");
                return;
            }

            Console.Clear();
            Interface.PrintLine("?", "Type the expected response indicating a successful XSS attack");
            var expectedResponse = Interface.ReadLine();

            if (string.IsNullOrEmpty(expectedResponse))
            {
                Interface.PrintLine("~", "Expected response cannot be empty.");
                return;
            }

            Xss xss = new(url);
            await xss.ScanXss(xssPayload, expectedResponse);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while scanning for XSS: {ex.Message}");
        }
    }

    private static void PayloadsGenerator()
    {
        try
        {
            Console.Clear();
            Interface.PrintLine("?", "Choose payload type:");
            foreach (var type in Enum.GetValues(typeof(Xss.Payload)))
                Interface.PrintLine(((int)type).ToString(), type.ToString() ?? throw new InvalidOperationException());

            var choice = Interface.ReadLine();
            if (!int.TryParse(choice, out var payloadTypeIndex) ||
                !Enum.IsDefined(typeof(Xss.Payload), payloadTypeIndex))
            {
                Interface.PrintLine("!", "Invalid choice. Please try again.");
                return;
            }

            var payloadType = (Xss.Payload)payloadTypeIndex;
            var xss = new Xss("???"); // URL is not used in payload generation
            var payloads = xss.ExamplePayloads(payloadType);

            if (!payloads.Any())
            {
                Console.WriteLine("No payloads generated.");
            }
            else
            {
                File.WriteToFile(File.GenerateFileName("XSS Payload"), payloads.ToList());
                Console.WriteLine("Payloads successfully written to file.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while generating payloads: {ex.Message}");
        }
    }
}