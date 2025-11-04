using HackerNews.WebAPI.Models;

namespace HackerNews.WebAPI.Interfaces;

public interface IHackerNewsService 
{
    Task<List<StoryDto>>GetBestStoriesAsync(int storiesCount);
}
