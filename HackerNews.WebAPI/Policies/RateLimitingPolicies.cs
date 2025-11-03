using HackerNews.WebAPI.Models;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using System.Threading.RateLimiting;

namespace HackerNews.WebAPI.Policies;

public static class RateLimitingPolicies
{

    public static void AddCustomRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            using var serviceProvider = services
                .BuildServiceProvider();

            var rateLimitingConfig = serviceProvider
                .GetRequiredService<IOptions<RateLimitingOptions>>()
                .Value;

            options.AddPolicy("FixedWindowPolicy", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: key => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = rateLimitingConfig.PermitLimit,
                        Window = TimeSpan.FromMinutes(rateLimitingConfig.WindowMinutes),
                        QueueLimit = rateLimitingConfig.QueueLimit,
                        QueueProcessingOrder = Enum.Parse<QueueProcessingOrder>(rateLimitingConfig.QueueProcessingOrder)
                    }));
        });
    }

    
}



