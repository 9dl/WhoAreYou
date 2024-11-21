using WhoAreYou.Helpers;
using WhoAreYou.Menu;
using WhoAreYou.Modules;
using File = WhoAreYou.Helpers.File;

namespace WhoAreYou;

internal static class Program
{
    private static async Task Main()
    {
        Console.Title = "WhoAreYou";
        var exitProgram = false;

        while (!exitProgram)
        {
            Console.Clear();
            Interface.PrintLine("?", "Enter your choice:");
            Interface.PrintLine("1", "LFI Menu");
            Interface.PrintLine("2", "RFI Menu");
            Interface.PrintLine("3", "Scrape Links");
            Interface.PrintLine("4", "TCP Listener");
            Interface.PrintLine("0", "Exit");

            var choice = Interface.ReadLine();

            switch (choice)
            {
                case "1":
                    await LfiMenu.Menu();
                    ReturnToMenu();
                    break;

                case "2":
                    await RfiMenu.Menu();
                    ReturnToMenu();
                    break;

                case "3":
                    await ScrapeLinksMenu();
                    ReturnToMenu();
                    break;

                case "4":
                    await ListenerMenu();
                    ReturnToMenu();
                    break;

                case "0":
                    exitProgram = true;
                    Console.WriteLine("Exiting...");
                    break;

                default:
                    Interface.PrintLine("!", "Invalid choice. Please try again.");
                    Interface.ReadLine();
                    break;
            }
        }
    }

    private static void ReturnToMenu()
    {
        Interface.PrintLine("!", "Press any key to return to the main menu...", false);
        Console.ReadKey();
    }

    private static async Task ScrapeLinksMenu()
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

            Interface.PrintLine("?", "Max Sites (0 for unlimited)");
            var maxSites = Interface.ReadLine();

            if (string.IsNullOrEmpty(url))
            {
                Interface.PrintLine("~", "Max Sites cannot be empty.");
                return;
            }

            Interface.PrintLine("?", "Check Host (true/false)");
            var checkHost = Interface.ReadLine();

            if (string.IsNullOrEmpty(checkHost))
            {
                Interface.PrintLine("~", "Check Host cannot be empty.");
                return;
            }

            Interface.PrintLine("?", "Check for Parameters (true/false)");
            var checkForParameters = Interface.ReadLine();

            if (string.IsNullOrEmpty(checkForParameters))
            {
                Interface.PrintLine("~", "Check for Parameters cannot be empty.");
                return;
            }

            var filePath = File.GenerateFileName("Scraped Links");

            var scraper = new Scraper(url, int.Parse(maxSites), bool.Parse(checkHost), bool.Parse(checkForParameters));
            var links = await scraper.ScrapeLinksAsync();

            foreach (var link in links)
            {
                Interface.PrintLine("+", link);
                File.AppendToFile(filePath, link);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while scraping links: {ex.Message}");
        }
    }

    private static async Task ListenerMenu()
    {
        try
        {
            Console.Clear();
            Interface.PrintLine("?", "Type Port");
            var port = Interface.ReadLine();

            if (string.IsNullOrEmpty(port))
            {
                Interface.PrintLine("~", "Port cannot be empty.");
                return;
            }

            Console.Clear();
            Listener listener = new(int.Parse(port));
            await listener.Listen();
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while listening: {ex.Message}");
        }
    }
}