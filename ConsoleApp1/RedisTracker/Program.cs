using ConsoleApp1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// Begin Building the App
var builder = new HostApplicationBuilder();

// Setup Configuration and Logging
// Services will be loaded by the runtime
builder.Configuration
    .AddJsonFile("appsettings.json", true)
    .AddEnvironmentVariables("GALAXY")
    .AddCommandLine(args);

builder.Logging.ClearProviders();
#if DEBUG
builder.Logging.AddDebug();
#endif

builder.Logging.AddConsole();

// TODO: If any other loggers are needed (file, App Insights, CloudWatch, etc.) add them here

// Register our services

// Register our Worker
builder.Services.AddHostedService<RedisPollingService>();

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

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