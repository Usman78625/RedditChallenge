namespace RedditTracker.Core;

public class RedditProcessor:IRedditProcessor {
    private readonly IRedditApiClient _client;
    
    public RedditProcessor(IRedditApiClient client) {
        _client = client;
    }

    public async Task PollSubreddit() {

        

    }
}
