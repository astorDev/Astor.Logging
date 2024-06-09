using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Astor.Logging.Tests;

[TestClass]
public class MicrosoftJsonConsoleLoggerShould
{
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
    
    public ILogger BuildSimpleIndentedLogger()
    {
        return new ServiceCollection()
            .AddLogging(l => l.AddJsonConsole(j => j.JsonWriterOptions = new() { Indented = true }))
            .BuildServiceProvider()
            .GetRequiredService<ILogger<object>>();
    }
}