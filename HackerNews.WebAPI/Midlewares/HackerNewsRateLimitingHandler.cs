using HackerNews.WebAPI.Models;
using Microsoft.Extensions.Options;

namespace HackerNews.WebAPI.Midlewares;

public class HackerNewsRateLimitingHandler(
    IOptions<HackerNewsOptions> options) : DelegatingHandler
{
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
            Console.WriteLine(ex);
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}