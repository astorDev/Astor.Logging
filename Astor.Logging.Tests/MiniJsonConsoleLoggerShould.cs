using System;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Astor.Logging.Tests;

[TestClass]
public class MiniJsonConsoleLoggerShould
{
    [TestMethod]
    public void LogOnlyNotNullStateFieldsByDefault()
    {
        var logger = new ServiceCollection()
            .AddLogging(l => l
                .AddMiniJsonConsole()
                .AddFilter((p, _, lev) => p!.Contains(MiniJsonConsoleLogger.Id) && lev >= LogLevel.Warning)
            )
            .BuildServiceProvider()
            .GetRequiredService<ILogger<MiniJsonConsoleLoggerShould>>();

        logger.LogWarning("{name} {age} {hobby}", "Egor", 27, new { Category = "board games", Favorite = "resistance"});
        logger.LogInformation("{greeting}, world!", "Hello");
    }
    
    [TestMethod]
    public void LogEverythingInKebabCaseIfConfiguredToDoSo()
    {
        var logger = new ServiceCollection()
            .AddLogging(l => l
                .AddMiniJsonConsole(o => o.IncludeAll().SetNamingPolicy(JsonNamingPolicy.KebabCaseLower))
                .AddFilter((p, _, lev) => p!.Contains(MiniJsonConsoleLogger.Id) && lev >= LogLevel.Warning)
            )
            .BuildServiceProvider()
            .GetRequiredService<ILogger<MiniJsonConsoleLoggerShould>>();

        logger.LogWarning("{name} {age} {hobby}", "Egor", 27, new { Category = "board games", Favorite = "resistance"});
        logger.LogInformation("{greeting}, world!", "Hello");
    }

    [TestMethod]
    public void LogANestedObjectFromTypelessDeserialization()
    {
        var logger = new ServiceCollection()
            .AddLogging(l => l
                .AddMiniJsonConsole(j => j.Indent())
            )
            .BuildServiceProvider()
            .GetRequiredService<ILogger<MiniJsonConsoleLoggerShould>>();

        var method = "POST";
        var user = new User(Guid.NewGuid().ToString(), "Andrew");
        var userJson = JsonSerializer.Serialize(user);
        var userX = JsonSerializer.Deserialize<object>(userJson);
        
        logger.LogInformation("Sending {method} with {requestBody}", method, userX);
    }
}

public record User(string Id, string Name);