using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsoleApp1;

public sealed class RedisPollingService :IHostedService, IDisposable {
    private readonly IConfiguration _config;
    private readonly ILogger<RedisPollingService> _logger;
    private Task? _worker;
    private bool _isRunning;

    public RedisPollingService(
        IConfiguration config,
        ILogger<RedisPollingService> logger
    ) {
        _config = config ?? throw new ArgumentNullException(nameof(config));
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
