~ 11:20
1. Read requirements.  Decided to implement project in .NET 6.0 per requirements, but recommend upgrading to .NET 8.0 ASAP as .NET 6.0 is listed for end-of-support on 11/12/24, while .NET 7.0 is listed to end support 5/14/24, only a few weeks away at the time of writing.  Using .NET 6.0 allows for a few more months of support.  Will add cross-compile options to make this transition smoother.

~ 11:30
2. Created core console app to host the project.  Decided to use a console, as it can harvest data in the background without needing a user to poll for information like a web service may require.  Theoretically, I could write a background service to poll Redis for the data and store it, while a REST or GraphQL front-end allows for querying of the data, but that seems outside the scope fo the requirements.

~ 11:36
3. Add docker file to console app.  This allows the console to be deployed to a k8 cluster or other hosted environment.  Because the app is configurable, we could scrape a large number of subredits at one time using multiple containers, allowing for a high degree of horizontal scalability.
4. Added IConfiguration and ILogger.  Created a hosted service.  Just because this is a small console is no excuse for sloppy code.  We aren't just dump stuff to a Console.Write, we want to be able to ship our information wherever makes sense for our infrastructure.

~ 11:40
5. Sign up for Redis Developer account and request access to free tier of the API.  This process is unusually long and complicated compared to similar tools.  Despite saying they want developers to create bots for the platform, there is a waitlist to get approvals.
6. Future versions of this exercise may want to consider using a different service with an easier path to access such as StackExchange or even the English language edits of Wikipedia.
7. Have no yet recieved access to the API, will begin coding with anticipation of approval.

~ 11:55
8. First code compiles, runs.

~ 12:20
9. Added Library for testing, added unit test project.  Restructure code, cleanup .gitignore, etc.
10. Add Reddit API client library

~ 12:38
11. Create an abstraction layer around the Reddit client.  I cannot count the number of times I've seen devs fail to do this, and therefore have no way to test the logic that gets the data from the remote API seperately from the logic that processes that data, making it impossible to test without a full integration to a live endpoint.  I'm not making this mistake.

~ 12:55
12. Re-read requirements to make sure that I am getting the data needed, and only the data needed.  No reason to send/receive extra bytes, and absolutely must have the required information.
13. Got approval to use the developer API.  Skimmed the Terms and Conditions-they basically want to make sure we are not going to abuse the system or use it for something illegal.  We aren't so we are ok.  Looking up rate limits to verify the "max it all out" requirement.
14. Reviewed documentation for Reddit API.

~ 13:10 - 13:30
14. Take a break to get lunch and collect my thoughts on architecture.  The Reddit API library has a built-in polling mechanism that will save a great deal of time.  In the real-world, I'd probably speak with the team to determine a poll is really the right solution in this case, or if a pub/sub model might be better (I'm not suggesting it isn't the correct solution, I would just want to verify with the team that we all agree where it fits into the bigger picture).  In any case, I'm going to design it in such a way that the core logic will be in a reusable DLL and hte logic that handles the polling/subscription can live in the seperate console app that only acts as a coordinator.  This way if we need to change out behaviors in the future, it is easy to do so.
15. The Reddit API library has the ability to set up monitors.  This creates a backend polling process that divides the number of watchers among the available number of connection requests and schedules them appropriately to maximize usage.  This saves a ton of work in building a request coordinator, so I am going to use this feature.  https://github.com/sirkris/Reddit.NET?tab=readme-ov-file#monitoring
16. Adding Polly Library to handle any simple networking issues that may crop up while we are reading from Reddit.  I am going to limit usage of this to the actual API/Data layer, not the business logic layer in order to seperate out the concerns of cleaning retrieving the data, and processing it.
17. The requirements mention that while there is no need to store the data at this time, consider how to do so in the future.  Therefore, I am going to create a storage interface, and provide a 'do nothing' dummy backend for now.  We can implement this interface to pump our data to SQL Server, SQLite, Redis, Kafka, RabbitMQ, DynamoDB, etc.  Basically any sink for our data.  We can control this by swapping out the class used for the DI.  We could even write a chunk of code to handle it dynamically based on configs depending on what our use case was (say SQLite for local development and PostgreSQL for production, although we could also solve this with Docker).

~ 13:45
18. The requirements are a little fuzzy with regard to how posts are ranked.  It seems to imply that we are tracking the posts with the most upvotes and the user with the most posts only during the time-window the application is running.  That is to say we are not looking at historical posts (i.e. the most upvotes of all time).  It's a little fuzzier as to if we should look at posts that were made prior to the application starting.  Given the requirement to track the users with the most posts (presumably during the time window the app is running), I'm going to assume that ANY upvote or ANY post to the particular subreddit needs to be tracked by the application.  We will then look at the posts those are applied to and do a simple count: net change during application runtime.  We can then organize and sort this data into a kind of "leaderboard."  For the sake of fun, we will default this to 10, but make it configurable for those who want a top 20 or top 100 list.

~ 15:45
19. Stop for the day to play with the kids during dad day.  Will pick back up tomorrow.

~ Day 2 11:00
20. Sat down for a second day.  Expecting to wrap up the code soon.  The code builds and runs, but the formatting is pretty ugly.  Going to clean that up, double check our regression tests and code coverage against the Core dll (the console app doesn't really contain code that is easily unit tested).  Lastly, I want to make sure the docker image is working correctly, since that is theoretically how we want to deploy this.  I'm not going to setup a build and deploy pipeline because I don't know what environment we are ultimately targeting, but using Docker as our host gives us a lot of options.