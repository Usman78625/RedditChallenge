using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Reddit;
using RedditTracker;
using RedditTracker.Core;
using RedditTracker.Core.Services;

// Begin Building the App
var builder = new HostApplicationBuilder();

// Setup Configuration and Logging
builder.Configuration.Sources.Clear();
builder.Configuration
    .AddJsonFile("appsettings.json", true) // We don't actually "need" a config file, but allow one.
    .AddEnvironmentVariables()  // We could set from env var (i.e. Docker, local dev)
    .AddCommandLine(args); // Or we can set from command line for various reasons (inline CLI)

builder.Logging.ClearProviders();
#if DEBUG
builder.Logging.AddDebug();
#endif

builder.Logging.AddConsole(options => {
    options.FormatterName = nameof(CleanConsoleFormatter);
}).AddConsoleFormatter<CleanConsoleFormatter, ConsoleFormatterOptions>();

// Note: If any other loggers are needed (file, App Insights, CloudWatch, etc.) add them here

// Register our services
builder.Services.AddTransient<TrackerSettings>((sp) => {
    // Read settings from config
    var settings = new TrackerSettings();
    var config = sp.GetRequiredService<IConfiguration>().GetSection("Tracker");
        config.Bind(settings);
    return settings;
});

builder.Services.AddScoped<RedditClient>((sp) => {
    // construct our API client
    var settings = sp.GetRequiredService<TrackerSettings>();
    var client= new RedditClient(appId:settings.AppId, appSecret:settings.AppSecret, accessToken:settings.RefreshToken);
    return client;
});

// NOTE: Choosing singletons here because of the way most data sinks work,
// although the actual use case may dictate a different lifetime.
builder.Services
    .AddSingleton<IUserStatisticsStorage, DummyUserStatisticsStorage>();
builder.Services
    .AddSingleton<IUpvoteStatisticsStorage, DummyUpvoteStatisticsStorage>();
builder.Services
    .AddSingleton<IUpvoteStatisticsManager, UpvoteStatisticsManager>();
builder.Services.AddSingleton<IUserStatisticsManager, UserStatisticsManager>();
builder.Services.AddSingleton<IRedditApiClient, RedditApiClient>();
builder.Services.AddSingleton<IRedditMonitor, RedditMonitor>();

// Register our Worker
builder.Services.AddHostedService<RedditPollingService>();

// Build the application
var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Run it
try {
    // Validate our settings here
    // I could probably use FluentValidator or some other library, but
    // since this is such a small app, we can do it all here,
    // but this would be difficult to maintain on a large system
    var settings = app.Services.GetRequiredService<TrackerSettings>();
    if (string.IsNullOrWhiteSpace(settings.AppSecret)) {
        throw new ArgumentNullException(nameof(TrackerSettings.AppSecret));
    }
    if (string.IsNullOrWhiteSpace(settings.RefreshToken)) {
        throw new ArgumentNullException(nameof(TrackerSettings.RefreshToken));
    }
    if (string.IsNullOrWhiteSpace(settings.AppId)) {
        throw new ArgumentNullException(nameof(TrackerSettings.AppId));
    }
    if (string.IsNullOrWhiteSpace(settings.Subreddit)) {
        throw new ArgumentNullException(nameof(TrackerSettings.Subreddit));
    }
    
    logger.LogInformation("Starting application.");
    app.Run();
}
catch (Exception ex) {
    logger.LogCritical(ex, "Critical Application Exception - {Message}", ex.Message);
}
finally {
    logger.LogInformation("Starting application.");
    app.Dispose();
}