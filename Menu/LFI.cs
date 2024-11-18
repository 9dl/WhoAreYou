using WhoAreYou.Helpers;
using WhoAreYou.Modules;
using File = WhoAreYou.Helpers.File;

namespace WhoAreYou.Menu;

public static class LfiMenu
{
    public static async Task Menu()
    {
        Console.Clear();
        Interface.PrintLine("LFI", "Local File Inclusion");
        Interface.PrintLine("?", "Choose an option:");
        Interface.PrintLine("1", "Generate Payloads");
        Interface.PrintLine("2", "Scan for LFI");
        Interface.PrintLine("0", "Exit");

        var choice = Interface.ReadLine();

        switch (choice)
        {
            case "1":
                PayloadsGenerator();
                break;
            case "2":
                await LfiScanner();
                break;
            default:
                Interface.PrintLine("!", "Invalid choice. Please try again.");
                Interface.ReadLine();
                break;
        }
    }

    private static async Task LfiScanner()
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
            Interface.PrintLine("?", "Type the path of the LFI payloads file");
            var lfiPayloadsPath = Interface.ReadLine();

            if (string.IsNullOrEmpty(lfiPayloadsPath))
            {
                Interface.PrintLine("~", "LFI Payloads file path cannot be empty.");
                return;
            }

            Lfi lfi = new(url);
            await lfi.ScanLfi(lfiPayloadsPath);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while generating payloads: {ex.Message}");
        }
    }

    private static void PayloadsGenerator()
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

            Lfi lfi = new(url);

            var payloads = Interface.ShowOptions(lfi.GeneratePayloads) as IEnumerable<string>;

            if (payloads == null || !payloads.Any())
            {
                Console.WriteLine("No payloads generated.");
            }
            else
            {
                File.WriteToFile(File.GenerateFileName("LFI Payloads"), payloads.ToList());
                Console.WriteLine("Payloads successfully written to file.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while generating payloads: {ex.Message}");
        }
    }
}