using WhoAreYou.Helpers;
using WhoAreYou.Modules;

namespace WhoAreYou.Menu;

public static class RfiMenu
{
    public static async Task Menu()
    {
        Console.Clear();
        Interface.PrintLine("LFI", "Remote File Inclusion");
        Interface.PrintLine("?", "Choose an option:");
        Interface.PrintLine("1", "Scan for RFI");
        Interface.PrintLine("2", "Generate a Example Payload for RFI");
        Interface.PrintLine("0", "Exit");

        var choice = Interface.ReadLine();

        switch (choice)
        {
            case "1":
                await RfiScanner();
                break;
            case "2":
                Rfi.RfiPayload();
                break;
            default:
                Interface.PrintLine("!", "Invalid choice. Please try again.");
                Interface.ReadLine();
                break;
        }
    }

    private static async Task RfiScanner()
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
            Interface.PrintLine("?", "Type Listener URL in this format: `?id=http://???.com/cmd.txt`");
            var listenerUrl = Interface.ReadLine();

            if (string.IsNullOrEmpty(url))
            {
                Interface.PrintLine("~", "Listener URL cannot be empty.");
                return;
            }

            Console.Clear();
            Interface.PrintLine("~", "If you use the built-in listener, the expected response is 'WhoAreYou?!'");
            Interface.PrintLine("?", "Type Expected Response from Listener");
            var expectedResponse = Interface.ReadLine();
            if (string.IsNullOrEmpty(url))
            {
                Interface.PrintLine("~", "Expected Response cannot be empty.");
                return;
            }

            Rfi rfi = new(url);
            await rfi.ScanRfi(listenerUrl, expectedResponse);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while generating payloads: {ex.Message}");
        }
    }
}