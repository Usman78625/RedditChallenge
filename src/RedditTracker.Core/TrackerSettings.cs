namespace RedditTracker.Core;

public class TrackerSettings {
    public string Subreddit { get; set; } = string.Empty;

    public string AppId { get; set; } = string.Empty;
    
    public string AppSecret { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;
}
