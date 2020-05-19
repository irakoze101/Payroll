# PLCT 2020

## Organization
The `old_demo` folder contains a simplified client-only app written first to explore writing a 
client using Blazor Webassembly, which doesn't persist data between runs, for posterity's sake.
The `Payroll` folder contains the finished submission, with a Blazor Webassembly client, a .NET Core
API, and unit tests.

## Building
The current solution will only build & run on Visual Studio 2019 Preview on Windows (with the "ASP.NET 
and web development" loadout), which can be installed side-by-side with the standard VS2019 using the
Visual Studio Installer. The solution builds and launches cross-platform with the latest .NET Core SDK, 
but the configured database only works on Windows, so it doesn't get far.

When you're done playing with it, go to your AppData\Local\Temp folder and delete any folders there named vscode-js-debug-xxxxxx.
I ended up with about 20 gigs' worth of those.

## Rationale
### Why Blazor?
It's a shiny new thing I haven't had much chance to play with yet, and my C# is much better 
than my JavaScript right now.
### But why Blazor _WebAssembly_?
The Blazor Server apps I've used frequently lose their connection with the server, which results
in the page dimming and displaying a "Reconnecting..." message constantly, even when nothing's happening.
I wanted to see how WebAssembly compared.
### Was it worth it?
Not really. The documentation is still pretty thin, bugs abound, and the debugging support is almost useless. 
Very little code was shared between the client and server projects.

## Afterthoughts
Writing these down while they're fresh in my mind.
* I've only enabled Nullable References on simpler coding exercises before. While EF Core doesn't
provide the nullability annotations yet, I did appreciate that using nullables for my navigation
properties forced me to acknowledge the potential for nullability when referring to them.
* I implemented support in the API for customizing pay periods per year by user/employer, but didn't
get around to implementing a UI for changing it. I shouldn't have written the API code unless I was
going to put in the UI - it just ended up complicating the cost calculations and testing for no
benefit. And if, hypothetically, a requirement was later added for customizing pay periods, it might
not match my implementation - what if pay periods need to be customized per-employee, not per-employer?
_Don't write it until you need it._
* I pulled the database access from the controllers into repo classes to simplify unit testing the
controllers, but hit another roadblock in mocking out the current UserId. Between the repos and the
BenefitsService, there's very little logic left in the controllers anyway.
* Originally I intended to pass database models to and from the client instead of DTOs, in the interest
of avoiding mapping bugs. Everything worked except for the dependents on the Update endpoint. 
While looking for a solution, I stumbled on a [convincing argument](https://stackoverflow.com/questions/55110021/ef-core-removing-a-related-entity-from-collection-navigation-property-does-not)
for just not doing it that way, so I added the DTOs, wrote some mappings and validation, and unit
tested the heck out of them.
* The patterns for providing unit test cases vary wildly between the test classes. It's been a little
while since I've written proper tests, so I played around with different approaches. I'm not sure
my attempt at generating an "exhaustive" set of employee models & DTOs for the mapping tests was
worth the effort, but it was fun.
* I spent so much time on the API unit tests that I didn't get around to exploring UI unit tests. Some
libraries do exist for unit testing Blazor components; being able to share test cases between server and
client would have helped justify both the time spent on the test cases and the choice of Blazor.
