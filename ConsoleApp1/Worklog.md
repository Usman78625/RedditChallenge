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
