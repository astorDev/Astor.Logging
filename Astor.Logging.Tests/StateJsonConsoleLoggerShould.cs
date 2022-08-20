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
            .AddLogging(l => l.AddStateJsonConsole()
                .AddFilter((provider, _, logLevel) => 
                    provider.Contains("StateJsonConsole") && logLevel >= LogLevel.Warning)
            )
            .BuildServiceProvider()
            .GetRequiredService<ILogger<StateJsonConsoleLoggerShould>>();

        logger.LogWarning("{name} {age} {hobby}", "Egor", 27, new { Category = "Board Games", Favorite = "Resistance"});
        logger.LogInformation("{greeting}, world!", "Hello");
    }
}