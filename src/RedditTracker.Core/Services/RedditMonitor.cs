namespace RedditTracker.Core;

public class RedditMonitor : IRedditMonitor {
    private readonly IRedditApiClient _client;
    private readonly IUserStatisticsManager _userStatisticsManager;
    private readonly IUpvoteStatisticsManager _upvoteStatisticsManager;
    private bool _isRunning;

    public RedditMonitor(
        IRedditApiClient client,
        IUserStatisticsManager userStatisticsManager,
        IUpvoteStatisticsManager upvoteStatisticsManager
    ) {
        _client = client;
        _userStatisticsManager = userStatisticsManager;
        _upvoteStatisticsManager = upvoteStatisticsManager;
    }

    public void StartMonitoring() {
        if (_isRunning) {
            throw new InvalidOperationException("Service is already running");
        }

        _isRunning = true;
        
        // Start the process of monitoring for new posts and building our statistics
        var newPosts = _client.ListAndMonitorNewPosts((posts)
            => _userStatisticsManager.UpdateData(posts));
        _userStatisticsManager.UpdateData(newPosts);

        // Start the process of monitoring hot posts
        var hotPosts = _client.ListAndMonitorHotPosts((posts)
            => _upvoteStatisticsManager.UpdateData(posts));
        _upvoteStatisticsManager.UpdateData(hotPosts);
    }

    public void StopMonitoring() {
        if (!_isRunning) {
            throw new InvalidOperationException("Service is not running");
        }

        _isRunning = false;
        
        _client.StopMonitoringNewPosts();
        _client.StopMonitoringHotPosts();
    }

    public void Dispose() {
        if (_isRunning) {
            StopMonitoring();
        }

        _client.Dispose();
    }
}
