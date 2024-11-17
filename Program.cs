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
            Interface.PrintLine("2", "Scrape Links");
            Interface.PrintLine("3", "Exit");

            var choice = Interface.ReadLine();

            switch (choice)
            {
                case "1":
                    await LfiMenu.Menu();
                    ReturnToMenu();
                    break;

                case "2":
                    await ScrapeLinksMenu();
                    ReturnToMenu();
                    break;

                case "3":
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

            var filePath = File.GenerateFileName("Scraped Links");

            var scraper = new Scraper(url, int.Parse(maxSites));
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
}