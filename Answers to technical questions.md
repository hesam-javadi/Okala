1. **How long did you spend on the coding assignment? What would you add to your solution if you had more time?**
   
   I spent approximately 14 hours on the assignment. If I had more time, I would:
   - Implement authorization using JWT to secure the API.
   - Integrate a database like MongoDB or PostgreSQL for persistent storage.
   - Improve the Redis integration, making it more robust and efficient for caching.
   - Add rate-limiting validation to prevent abuse and ensure better API reliability.
   - Utilize Hangfire to create a recurring background job that fetches exchange rates for commonly used cryptocurrencies like Bitcoin and Ethereum. This would enhance the API's performance by reducing dependency on live API calls for frequently requested data.
   - Include a UML Diagram in the README file to visualize the relationships between objects and provide a clear overview of the project's architecture. (I've added its section to the readme file, but I didn't have time to create the diagram 😢).
2. **What was the most useful feature that was added to the latest version of your language of choice? Please include a snippet of code that shows how you've used it.**
   
   I think one of the most useful features in .NET 8 is the introduction of DateOnly and TimeOnly data types, which allow for precise handling of date and time without relying on DateTime when the extra information isn't needed. This improves clarity and reduces potential bugs caused by irrelevant data.

	 Here’s an example of using DateOnly and TimeOnly in a scheduling context:

	 ```csharp
    public class Schedule
    {
        public DateOnly StartDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    
        public Schedule(DateOnly startDate, TimeOnly startTime, TimeOnly endTime)
        {
            StartDate = startDate;
            StartTime = startTime;
            EndTime = endTime;
        }
    
        public override string ToString()
        {
            return $"Schedule starts on {StartDate} from {StartTime} to {EndTime}";
        }
    }

   ```

  3. **How would you track down a performance issue in production? Have you ever had to do this?**

     I've encountered performance issues in low-scale projects before and generally follow these steps to identify and resolve them:

     - **Identify Bottlenecks:**

       I start by analyzing the application to identify potential bottlenecks that could slow down the process, such as inefficient database queries or slow API calls.
     - **Debug the Code:**

       For straightforward problems, I use the built-in debugging tools in my IDE to step through the code and inspect variable states and execution flow.

     - **Add Logging for Clarity:**

       If the issue is not immediately apparent, I introduce logging (using tools like Serilog or similar) at strategic points in the code that seem suspicious. This helps me gather more information about the application's behavior during runtime.

     - **Simplify the Environment:**

       For complex or hard-to-find problems, I isolate the problematic code and run it in a simpler environment, like a console application. This helps me rule out external factors and focus on the core logic.

  4. **What was the latest technical book you have read or tech conference you have been to? What did you learn?**

     I recently read Alexander Shvets' Dive into Design Patterns. It transformed my approach to designing software by offering practical insights into implementing design patterns. I particularly enjoyed the hands-on examples and discussions on patterns like Strategy, Factory, and Observer, which I have since applied to make my code more flexible and maintainable. It reinforced the importance of understanding SOLID principles in software architecture.

  5. **What do you think about this technical assessment?**

     This project is an excellent test for evaluating a developer's technical skills, architectural understanding, and ability to integrate external APIs. However, it could be improved by including scenarios that involve interaction with a database (SQL or NoSQL) to test data modeling and query-building skills.

  6. **Please, describe yourself using JSON.**

     ```json
     {
       "id": "0250375631", // National code, LOL :)
       "name": "Hesam Javadi",
       "type": "Human",
       "subType": "Nerd Developer",
       "happinessPercent": 75, // work (25%) + Sleep (25%) + Game (25%) + University (0%)
       "routine": [
         {
           "index": 0,
           "name": "Work",
           "isPreferred": true,
           "Activities": [
             "Coding",
             "Learning",
             "Refueling with Coffee",
             "Communication"
           ]
         },
         {
           "index": 1,
           "name": "Sleep",
           "isPreferred": true,
           "Activities": [
             "Dreaming",
             "Some other stuff"
           ]
         },
         {
           "index": 2,
           "name": "University",
           "isPreferred": false,
           "Activities": [
             "Wasting time on the way",
             "Wasting time in classes",
             "Wasting time on the way back"
           ]
         },
         {
           "index": 3,
           "name": "Game",
           "isPreferred": true,
           "Activities": [
             "Kill bosses",
             "Find some easter eggs",
             "Enjoy the story"
           ]
         }
       ]
     }
     ```
