using HackerNews.WebAPI.Models;
using Microsoft.Extensions.Options;

namespace HackerNews.WebAPI.Midlewares;

public class HackerNewsRateLimitingHandler(
    IOptions<HackerNewsOptions> options,
    ILogger<HackerNewsRateLimitingHandler> logger
    ) : DelegatingHandler
{
    private readonly ILogger<HackerNewsRateLimitingHandler> _logger = logger;
    private readonly SemaphoreSlim _semaphore = new(options.Value.MaxConcurrentRequests);

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        await _semaphore
            .WaitAsync(cancellationToken);

        try
        {
            return await base
                .SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while processing request to {Uri}", request.RequestUri);
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}