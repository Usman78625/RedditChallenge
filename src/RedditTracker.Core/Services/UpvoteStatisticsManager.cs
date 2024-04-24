namespace RedditTracker.Core;

public sealed class UpvoteStatisticsManager:IUpvoteStatisticsManager {
    private readonly IUserStatisticsStorage _storage;
    private readonly ILogger<UserStatisticsManager> _logger;
    private readonly TrackerSettings _settings;

    private readonly IDictionary<string, ICollection<string>> _authorCounts =
        new Dictionary<string, ICollection<string>>();
    private class VotingRecord {
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
    }
    
    public void UpdateData(List<Post> posts) {
        _logger.LogDebug("Processing incoming Data");
        var didChangeData = false;
        foreach (var post in posts) {
            post.
        }
        
        if (didChangeData) {
            _logger.LogDebug("Updating statistics");
            RecomputeStatistics();
        }
    }
    
    private void RecomputeStatistics() {
        var topPosters = _authorCounts
            .Select(kv => new {
                Author = kv.Key, NumPosts = kv.Value.Count
            }) // Project into a simple type
            .OrderByDescending(i => i.NumPosts) // Top down
            .Take(_settings
                .TopUserLeaderboardSize) // Our configurable leaderboard size.
            .Enumerate((data, index) => new UpvoteStatisticsRecord(data.Author,
                data.NumPosts, index, DateTime.UtcNow)) // Always use UTC
            .ToArray(); // We could technically just forward all this on.
        _storage.UpdateStatistics(topPosters);
    }
}
