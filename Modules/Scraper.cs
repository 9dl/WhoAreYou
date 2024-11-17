using System.Text.RegularExpressions;

namespace WhoAreYou.Modules;

internal class Scraper
{
    private readonly bool _checkHost;
    private readonly int _maxSites;
    private readonly string _url;

    /// <param name="maxSites">If 0 is passed as maxSites, it will scrape all links.</param>
    /// <param name="url">The URL to scrape links from.</param>
    /// <param name="checkHost">Check if domain matches with the link it scraped.</param>
    public Scraper(string url, int maxSites = 10, bool checkHost = false)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL cannot be null or empty.", nameof(url));

        _url = url;
        _maxSites = maxSites;
        _checkHost = checkHost;
    }

    private async Task<string> FetchHtmlAsync()
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(_url);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Failed to fetch HTML. Status code: {response.StatusCode}");

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<List<string>> ScrapeLinksAsync()
    {
        var htmlContent = await FetchHtmlAsync();
        var urlPattern = @"href\s*=\s*[""'](https?://.*?)(?=[""'])";

        var matches = Regex.Matches(htmlContent, urlPattern);
        var links = new List<string>();
        Uri.TryCreate(_url, UriKind.Absolute, out var newUri);

        if (newUri == null)
            return links;

        foreach (Match match in matches)
            if (match.Success && !links.Contains(match.Groups[1].Value) && (links.Count < _maxSites || _maxSites == 0))
            {
                var link = match.Groups[1].Value;
                if (_checkHost)
                {
                    Uri.TryCreate(link, UriKind.Absolute, out var uri);
                    if (uri != null && uri.Host == newUri.Host)
                        links.Add(link);
                }
                else
                {
                    links.Add(link);
                }
            }

        return links;
    }
}