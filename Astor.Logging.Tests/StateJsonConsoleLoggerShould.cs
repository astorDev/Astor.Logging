using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Astor.Logging.Tests;

[TestClass]
public class StateJsonConsoleLoggerShould
{
    [TestMethod]
    public void LogOnlyNotNullStateFields()
    {
        var logger = new ServiceCollection()
            .AddLogging(l => l.AddStateJsonConsole(s => s.PropertyNamingPolicy = JsonNamingPolicy.CamelCase)
                .AddFilter((provider, _, logLevel) => 
                    provider.Contains("StateJsonConsole") && logLevel >= LogLevel.Warning)
            )
            .BuildServiceProvider()
            .GetRequiredService<ILogger<StateJsonConsoleLoggerShould>>();

        logger.LogWarning("{name} {age} {hobby}", "Egor", 27, new { Category = "board games", Favorite = "resistance"});
        logger.LogInformation("{greeting}, world!", "Hello");
    }
}