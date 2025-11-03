using HackerNews.WebAPI.Interfaces;
using HackerNews.WebAPI.Midlewares;
using HackerNews.WebAPI.Models;
using HackerNews.WebAPI.Policies;
using HackerNews.WebAPI.Services;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

// Add Options
var hackerNewsOptions = builder.Configuration
    .GetSection(nameof(HackerNewsOptions))
    .Get<HackerNewsOptions>() ?? new();

builder.Services.Configure<HackerNewsOptions>(
    builder.Configuration.GetSection(nameof(HackerNewsOptions)));

builder.Services.Configure<RateLimitingOptions>(
    builder.Configuration.GetSection(nameof(RateLimitingOptions)));


// Add http clients.

builder.Services.AddTransient<HackerNewsRateLimitingHandler>();

builder.Services.AddHttpClient(hackerNewsOptions.HttpClientName, client =>
{
    client.Timeout = TimeSpan.FromSeconds(hackerNewsOptions.TimeOutSeconds);

    client.BaseAddress = new Uri(hackerNewsOptions.BaseUrl);
    
}).AddHttpMessageHandler<HackerNewsRateLimitingHandler>()
.AddPolicyHandler((serviceProvider,request) =>
{
    var options = serviceProvider.GetRequiredService<IOptions<HackerNewsOptions>>();

    return options is null
        ? throw new InvalidOperationException($"{nameof(HackerNewsOptions)} not registered in DI.")
        : HttpClientPolicies.GetRetryPolicy(options);
});


// Add services to the container.

builder.Services.AddTransient<IHackerNewsService, HackerNewsService>();

// Add rate limiting policies.
builder.Services.AddCustomRateLimiting();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
