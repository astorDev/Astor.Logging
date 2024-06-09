using System.Text.Json;
using Astor.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var logger = new ServiceCollection()
    .AddLogging(l => l.AddMiniJsonConsole(j => 
        j.IncludeAll().Indent().SetNamingPolicy(JsonNamingPolicy.KebabCaseLower)))
    .BuildServiceProvider()
    .GetRequiredService<ILogger<Program>>();

var egor = new Person("Egor", 28);
var hobby = JsonSerializer.Deserialize<object>("{\"Name\":\"Board Games\"}");

logger.LogInformation("{Person} {Hobby}", egor, hobby);
await Task.Delay(100); // Wait for logs to be written

record Person(string Name, int Age);