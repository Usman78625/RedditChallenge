using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RedditTracker.Core;

namespace RedditTracker;

public sealed class RedditPollingService :IHostedService, IDisposable {
    private readonly TrackerSettings _settings;
    private readonly ILogger<RedditPollingService> _logger;
    private Task? _worker;
    private bool _isRunning;

    public RedditPollingService(
        TrackerSettings settings,
        ILogger<RedditPollingService> logger
    ) {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task StartAsync(CancellationToken cancellationToken) {
        _logger.LogInformation("Starting Process");
        _worker = Task.Run(Work, cancellationToken);
        _isRunning = true;
        return Task.CompletedTask;
        
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        _logger.LogInformation("Stopping Process");
        _isRunning = false;
        _worker?.Wait(cancellationToken);
        return Task.CompletedTask;
    }

    private void Work() {
        _logger.LogInformation("Doing Work");
        while (_isRunning) {
            
        }
    }

    public void Dispose() {
        _worker?.Dispose();
    }
}
