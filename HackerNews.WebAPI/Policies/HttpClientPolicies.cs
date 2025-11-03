using HackerNews.WebAPI.Models;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace HackerNews.WebAPI.Policies;

public static class HttpClientPolicies
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(
    IOptions<HackerNewsOptions> options)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError() // Handles 5xx and 408 errors
            .WaitAndRetryAsync(
                retryCount: options.Value.RetryCount,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: static (outcome, timespan, retryAttempt, context) =>
                {
                    if (outcome.Exception is not null)
                    {
                        Console.WriteLine($"Retry {retryAttempt} due to exception: {outcome.Exception.Message}. Waiting {timespan.TotalSeconds}s before next try.");
                    }
                    else
                    {
                        Console.WriteLine($"Retry {retryAttempt} due to HTTP status code {(int)outcome.Result.StatusCode}. Waiting {timespan.TotalSeconds}s before next try.");
                    }

                });
    }
}