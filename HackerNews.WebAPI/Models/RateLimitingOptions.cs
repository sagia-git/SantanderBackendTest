namespace HackerNews.WebAPI.Models;

public class RateLimitingOptions
{
    public int PermitLimit { get; set; }
    public int WindowMinutes { get; set; }
    public int QueueLimit { get; set; }
    public string QueueProcessingOrder { get; set; } = null!;
}
