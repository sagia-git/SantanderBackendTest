using HackerNews.WebAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace HackerNews.WebAPI.Policies;

public static class HttpClientPolicies
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(
    IOptions<HackerNewsOptions> options,
    ILogger<HttpResponseMessage> logger)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError() // Handles 5xx and 408 errors
            .WaitAndRetryAsync(
                retryCount: options.Value.RetryCount,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {

                    if (outcome.Exception is not null)
                    {
                        logger.LogWarning(outcome.Exception,
                            "Retry {RetryAttempt} due to exception. Waiting {Delay}s before next try.",
                            retryAttempt,
                            timespan.TotalSeconds);
                    }
                    else
                    {
                        logger.LogWarning(
                            "Retry {RetryAttempt} due to HTTP status code {StatusCode}. Waiting {Delay}s before next try.",
                            retryAttempt,
                            (int)outcome.Result.StatusCode,
                            timespan.TotalSeconds);
                    }
                });
    }
}