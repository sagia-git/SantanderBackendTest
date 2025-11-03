using HackerNews.WebAPI.Interfaces;
using HackerNews.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HackerNews.WebAPI.Controllers;

[ApiController]
[Route("api/v0/[controller]")]
[EnableRateLimiting("FixedWindowPolicy")]

public class StoriesController(
    IHackerNewsService hackerNewsService
    ) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<StoryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<StoryDto>>> GetBestStoriesAsync(
        [FromQuery] int storiesCount)
    {

        List<StoryDto> result = await hackerNewsService
            .GetBestStoriesAsync(storiesCount);

        return Ok(result);
    }
}

