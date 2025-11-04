using HackerNews.WebAPI.Interfaces;
using HackerNews.WebAPI.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace HackerNews.WebAPI.Services;

public class HackerNewsService(
        IHttpClientFactory httpClientFactory,
        IOptions<HackerNewsOptions> options,
        ILogger<HackerNewsService> logger
    ) : IHackerNewsService
{
    private readonly ILogger<HackerNewsService> _logger = logger;

    public async Task<List<StoryDto>> GetBestStoriesAsync(int storiesCount)
    {
        var httpClient = httpClientFactory
            .CreateClient(options.Value.HttpClientName);

        // Retrieve best n stories
        var storyIds = await FetchBestStoryIdsAsync(
            httpClient,
            options.Value.BestStoriesEndpoint,
            storiesCount);

        // Retrieve stories details
        var tasks = GetStoryDetailsRequestTasks(            
            httpClient,
            options.Value.StoryDetailEndpoint,
            storyIds);

        
        List<StoryDto> stories = [.. (await Task.WhenAll(tasks))
            .Where(s => s is not null)];

        return stories;

    }


    private async Task<IEnumerable<int>> FetchBestStoryIdsAsync(
        HttpClient httpClient,
        string bestStoriesEndpoint,
        int storiesCount)
    {
        try
        {
            var idsResponse = await httpClient
                .GetStringAsync(bestStoriesEndpoint);

            var storyIds = JsonConvert
                .DeserializeObject<List<int>>(idsResponse)?
                .Take(storiesCount)
                .ToList() ?? [];

            return storyIds;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch best story IDs from {endpoint}", bestStoriesEndpoint);
            throw;
        }
    }


    private IEnumerable<Task<StoryDto?>> GetStoryDetailsRequestTasks(        
        HttpClient httpClient,
        string storyDetailEndpoint,
        IEnumerable<int> storyIds)
    {
        return storyIds.Select(async storyId =>
        {
            try
            {
                var storyEndpoint = string.Format(storyDetailEndpoint, storyId);

                var response = await httpClient
                    .GetAsync(storyEndpoint);

                response.EnsureSuccessStatusCode();

                var json = await response.Content
                    .ReadAsStringAsync();

                var story = JsonConvert
                    .DeserializeObject<StoryDto>(json);

                return story;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch story details for ID {storyId}", storyId);
                return null;
            }
        });
    }
}