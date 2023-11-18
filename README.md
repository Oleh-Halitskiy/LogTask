Architecture: 
I decided to keep roughly the same architecture present in the class library when it was given to me. 
That way we have LogLine and LogManager, with addition of an IClock interface.

LogLine roughly stayed the same, except I added the ToString method, also it now implements ILogLine, so that maybe we will have different logs.
Although I thought that LogManager doesn't accept LogLine but it accepts strings. From one side of the coin, we can just toss it a string and call it a day, easy, from the other side of the coin, maybe we want to pass different types of log, hence why we have ILogLine interface. In the end I chose to have easier log writing opposed to having to create an object, setDate, text, etc.

LogManager (before I think it was LogAsync) is now more readable and maintainable. I also added the ability to LogManager to record any errors if they happen in the main loop. I think I completely refactored this class, just leaving the main loop idea. Also added the ability to have a custom clock so we can test the component and maybe have some custom clock passed to the object.

SystemClock : IClock, this class I have just to have custom clock passed to it, maybe if we want to follow some specific time we can create custom clock that wouldn't just do DateTime.Now, something maybe like SpainClock and have Like DateTime.Now.Hour + 3 or something.

Just in general I tried to be "Don't Repeat Yourself" so I wrote a simple file manager that would allow us to create and write to files. I also added custom exception that I end up not using much, LogNotAcceptedException, we use it when LogManager not accepting new logs but we still want to write something

Also added XML type documentation for the code, so that we can use something like [docfx](https://dotnet.github.io/docfx/)I didn't document the tests, I think they're self-explanatory, people rarely read documentation themselfs let alone boring tests :D

Unit tests:
I hope they cover all major things in the class library. 

I have MockClock in there to fake time for testing LogManager ability to create a new file if we pass midnightI have tests for FileManager, LogLine and LogManager. LogManager covers everything that was requested in .PDF file with this task. 


