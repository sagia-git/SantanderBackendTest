using HackerNews.WebAPI.Converters;
using Newtonsoft.Json;

namespace HackerNews.WebAPI.Models;

public class StoryDto
{
    [JsonRequired]
    [JsonProperty("title")]
    public string Title { get; set; } = null!;

    [JsonRequired]
    [JsonProperty("url")]
    public string Uri { get; set; } = null!;

    [JsonRequired]
    [JsonProperty("by")]
    public string PostedBy { get; set; } = null!;


    [JsonRequired]
    [JsonProperty("time")]
    [JsonConverter(typeof(UnixTimeConverter))]
    public DateTimeOffset Time { get; set; }


    [JsonRequired]
    [JsonProperty("score")]
    public int Score { get; set; }

    [JsonRequired]
    [JsonProperty("descendants")]
    public int CommentCount { get; set; }
}
