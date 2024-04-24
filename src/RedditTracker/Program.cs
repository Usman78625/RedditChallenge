using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Reddit;
using RedditTracker;
using RedditTracker.Core;

// Begin Building the App
var builder = new HostApplicationBuilder();

// Setup Configuration and Logging
// Services will be loaded by the runtime
builder.Configuration
    .AddJsonFile("appsettings.json", true) // We don't actually "need" a config file, but allow one.
    .AddEnvironmentVariables("REDDIT")  // We could set from env var (i.e. Docker)
    .AddCommandLine(args); // Or we can set from command line for various reasons (inline CLI)

builder.Logging.ClearProviders();
#if DEBUG
builder.Logging.AddDebug();
#endif

builder.Logging.AddConsole();

// TODO: If any other loggers are needed (file, App Insights, CloudWatch, etc.) add them here

// Register our services
builder.Services.AddTransient<TrackerSettings>((sp) => {
    // Read settings from config
    var settings = new TrackerSettings();
    sp.GetRequiredService<IConfiguration>().GetSection("Tracker")
        .Bind(settings);
    return settings;
});

builder.Services.AddScoped<RedditClient>((sp) => {
    // construct our API client
    var settings = sp.GetRequiredService<TrackerSettings>();
    return new RedditClient(settings.AppId, settings.AppSecret, settings.RefreshToken);
});
builder.Services.AddScoped<IRedditApiClient, RedditApiClient>();
builder.Services.AddScoped<IRedditProcessor, RedditProcessor>();

// Register our Worker
builder.Services.AddHostedService<RedditPollingService>();

// Build the application
var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Run it
try {
    logger.LogInformation("Starting application.");
    app.Run();
}
catch (Exception ex) {
    logger.LogCritical(ex, "Critical Application Exception");
}
finally {
    logger.LogInformation("Starting application.");
    app.Dispose();
}