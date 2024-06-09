namespace Astor.Logging.Tests;

[TestClass]
public class MiniJsonConsoleLoggerShould
{
    public ILogger BuildSimpleIndentedLogger() =>
        new ServiceCollection()
            .AddLogging(l => l.AddMiniJsonConsole(c => c.Indent()))
            .BuildServiceProvider()
            .GetRequiredService<ILogger<object>>();

    [TestMethod]
    public void LogSimple()
    {
        var logger = this.BuildSimpleIndentedLogger();
        logger.LogInformation("{Name} {Age}", "Egor", 27);
    }

    [TestMethod]
    public void LogInnerAnonymous()
    {
        var logger = this.BuildSimpleIndentedLogger();
        logger.LogInformation("{Name} {Hobby}", "Egor", new { Name = "Board Games", Favorite = "Resistance" });
    }

    [TestMethod]
    public void LogWithExceptionAndDeserializedObject()
    {
        var logger = this.BuildSimpleIndentedLogger();
        
        var hobby = JsonSerializer.Deserialize<object>(Hobby.Json);
        
        logger.LogError(ExceptionGenerator.Generate(), "fail of {Name} {Hobby}", "Egor", hobby);
    }

    [TestMethod]
    public void LogAllFieldsInCombinedScenario()
    {
        var logger = new ServiceCollection()
            .AddLogging(l => l.AddMiniJsonConsole(c => c.Indent().IncludeAll()))
            .BuildServiceProvider()
            .GetRequiredService<ILogger<object>>();
        
        logger.LogError(ExceptionGenerator.Generate(),"fail of {Name} {Hobby}", "Egor", new { Name = "Board Games", Favorite = "Resistance" });
    }

    [TestMethod]
    public void LogRecord()
    {
        var logger = this.BuildSimpleIndentedLogger();
        
        logger.LogInformation("{Name} {Hobby}", "Egor", Hobby.Example1);
    }
}