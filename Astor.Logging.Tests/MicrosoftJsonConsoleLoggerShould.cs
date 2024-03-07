using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Astor.Logging.Tests;

[TestClass]
public class MicrosoftJsonConsoleLoggerShould
{
    [TestMethod]
    public void LogALotOfStuff()
    {
        var logger = new ServiceCollection()
            .AddLogging(l => l.AddJsonConsole(o => o.IncludeScopes = false)
                .AddFilter((provider, _, logLevel) => 
                    provider.Contains("Console") && logLevel >= LogLevel.Warning)
            )
            .BuildServiceProvider()
            .GetRequiredService<ILogger<MiniJsonConsoleLoggerShould>>();

        logger.LogWarning("{name} {age} {hobby}", "Egor", 27, new { Category = "board games", Favorite = "resistance"});
        logger.LogInformation("{greeting}, world!", "Hello");
    }
}