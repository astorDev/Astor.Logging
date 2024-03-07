# Astor.Logging

Home for a minimalistic alternative to `JsonConsole` logger.

```csharp
logging.AddMiniJsonConsole();

logger.LogWarning("{name} {age} {hobby}", "Egor", 27, new { Category = "board games", Favorite = "resistance"});
// Output: {"name":"Egor","age":27,"hobby":{"category":"board games","favorite":"resistance"}}
```

## Installation

```sh
dotnet add package Astor.Logging
```

## Comparison

Here's what you get with Microsoft's `JsonConsole` logger:

```json
{
    "EventId": 0,  // omitted by default in MiniJsonConsole
    "LogLevel": "Warning",  // omitted by default in MiniJsonConsole
    "Category": "Astor.Logging.Tests.MiniJsonConsoleLoggerShould", // omitted by default in MiniJsonConsole
    "Message": "Egor 27 { Category = board games, Favorite = resistance }", // Redundant calculated value #1
    "State": {
        "Message": "Egor 27 { Category = board games, Favorite = resistance }", // Redundant calculated value #2
        "name": "Egor",
        "age": 27,
        "hobby": "{ Category = board games, Favorite = resistance }", // Inner data treated as string
        "{OriginalFormat}": "{name} {age} {hobby}" // <- Do you need this?
    }
}
```

And this is what you get by default with `MiniJsonConsole`

```json
{
    "name": "Egor",
    "age": 27,
    "hobby": {
        "category": "board games",
        "favorite": "resistance"
    }
}
```