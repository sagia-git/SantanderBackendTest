namespace HackerNews.WebAPI.Models;
public class HackerNewsOptions
{
    public int MaxConcurrentRequests { get; set; }

    public string HttpClientName { get; set; } = null!;

    public string BaseUrl { get; set; } = null!;

    public string BestStoriesEndpoint { get; set; } = null!;

    public string StoryDetailEndpoint { get; set; } = null!;

    public int RetryCount { get; set; }

    public int TimeOutSeconds { get; set; }
}