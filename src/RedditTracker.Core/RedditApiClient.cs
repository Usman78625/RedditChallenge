using Reddit.Controllers;

namespace RedditTracker.Core;


public class RedditApiClient:IRedditApiClient {
    private readonly Subreddit _subreddit;
    
    public RedditApiClient(Reddit.RedditClient apiClient, TrackerSettings settings) {
        _subreddit=apiClient.Subreddit(settings.Subreddit);

    }

    public List<Post> ListHotPosts() {
       _subreddit.m
    }

}
