# Notes and Afterthoughts

This was a short exercise that was supposed to demonstrate something production worthy given the requirements on hand.  I'm not much of a redditer, so it was a bit of a learning experience as well as fun to dive into something new.  While this may seem like a light-hearted exercise, applications of this sort are exactly what is needed in modern data pipelines and microservice architectures.  Low-investment, nearly throw away applications that can be easily dropped into place behind an app gateway or as part of a pipeline give a lot more architectural flexibility while keeping costs very low.  There would be very little practical time savings in adding this same code to an existing application, while a well planned stand-alone is valuable in more situations.

## Thoughts about the exercise

### Time
The original exercise said it was expected to take 4-6 hours.  This would probably be correct if not for a few caveats:
- __Documentation__: Documentation to the level of detail in this project takes time, especially writing the worklog and planning notes as I go.
- __Installing Tools__: I ran into an issue with older .NET SDK versions not wanting to play nice with the newer ones, so I had to reinstall them in a different location.
- __Dockerization__: Choosing to containerize the app took a little extra time.

Overall the project took about 8 hours split up over two days.  I'd estimate about 5 of that was spent on technical tasks, with the remaining 3 on research and documentation.  Of those 5 hours, there were fairly evenly split between writing new code, testing and debugging.  In a larger project, I'd expect that more time to be spent debugging, but a project this small has a higher percentage of boilerplate functionality (logging, etc.) to functional code than most.

### Requirements
The requirements were well written, although there were a few areas where differences of interpretation might arise.  I'd want to talk those out with the BA or product owner before working on a real project.

### Choices along the way
- Although the exercise wanted to see examples of async programming, there really isn't much in this code.  The reason is that most of the threading, polling, error handling, etc. is performed by the nuget library I chose.  Although it meant I didn't have as much chance to show off certain skills, I believe this was the right call to make for this project, as it meets the requirements with the minimum amount of development effort without compromising quality, performance, maintainability or security.
- I spent a little extra effort to make sure the system was very configurable, as well as added enterprise logging.  I wanted to make sure this was something I would put on my own Cloud (if I had one).
- I didn't have time to set up any kind of CI/CD pipeline.  It would have been nice to actually deploy it to a test cloud account.  Along the same lines, I pushed the code directly to the main branch rather than going through a PR process.  If time permitted, it would be nice to demonstrate those as well.
- Given the short deadline to be off .Net 6 and 7, I choose to add .Net 8 as a build option.  I felt this was important, as I would not want to turn in code that would immediately have to be updated.  This may require anyone who does not have .Net 8.0 installed to edit the .csproj files to remove that build target.  Alternatively, the Docker container can be used, as it will compile correctly to any of the three versions.

## Ways the system could be improved


## Notes

### Build notes
This application was built using JetBrains Rider 2024.1 on MacOS Monterey with an Intel x64 processor.  The code was designed to be cross-platform and should work on any system with the appropriate frameworks, but has not been tested on any other hardware.