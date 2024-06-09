# Simple

```json
{
  "EventId": 0,
  "LogLevel": "Information",
  "Category": "object",
  "Message": "Egor 27",
  "State": {
    "Message": "Egor 27",
    "Name": "Egor",
    "Age": 27,
    "{OriginalFormat}": "{Name} {Age}"
  }
}
```

```json
{
  "name": "Egor",
  "age": 27
}
```

# Inner

```json
{
  "EventId": 0,
  "LogLevel": "Information",
  "Category": "object",
  "Message": "Egor { Name = Board Games, Favorite = Resistance }",
  "State": {
    "Message": "Egor { Name = Board Games, Favorite = Resistance }",
    "Name": "Egor",
    "Hobby": "{ Name = Board Games, Favorite = Resistance }",
    "{OriginalFormat}": "{Name} {Hobby}"
  }
}
```

```json
{
  "name": "Egor",
  "hobby": {"name":"Board Games","favorite":"Resistance"}
}
```

# Exception

```json
{
  "EventId": 0,
  "LogLevel": "Error",
  "Category": "object",
  "Message": "fail of Egor {\n \u0022Name\u0022 : \u0022Games\u0022,\n \u0022fav\u0022 : \u0022Ticket to ride\u0022\n}",
  "Exception": "System.InvalidOperationException: Expected Exception number ed8bc236-a646-48be-b20c-fb8541aeb0d5. \n   at Astor.Logging.Tests.ExceptionGenerator.Generate() in /Users/egortarasov/repos/Astor.Logging/Astor.Logging.Tests/ExceptionGenerator.cs:line 12",
  "State": {
    "Message": "fail of Egor {\n \u0022Name\u0022 : \u0022Games\u0022,\n \u0022fav\u0022 : \u0022Ticket to ride\u0022\n}",
    "Name": "Egor",
    "Hobby": "{\n \u0022Name\u0022 : \u0022Games\u0022,\n \u0022fav\u0022 : \u0022Ticket to ride\u0022\n}",
    "{OriginalFormat}": "fail of {Name} {Hobby}"
  }
}
```

```json
{
  "name": "Egor",
  "hobby": {"Name":"Games","fav":"Ticket to ride"},
  "logException": "System.InvalidOperationException: Expected Exception number 7ec23676-a9cf-4df2-93c0-133f1cb7224e. \n   at Astor.Logging.Tests.ExceptionGenerator.Generate() in /Users/egortarasov/repos/Astor.Logging/Astor.Logging.Tests/ExceptionGenerator.cs:line 12"
}
```


```json
{
  "name": "Egor",
  "hobby": {"name":"Board Games","favorite":"Resistance"},
  "logOriginalFormat": "fail of {Name} {Hobby}",
  "logCategoryName": "object",
  "logLevel": "Error",
  "logEventId": 4,
  "logException": "System.InvalidOperationException: Expected Exception number c313c8f6-c732-4805-9207-b2b937151a9c. \n   at Astor.Logging.Tests.ExceptionGenerator.Generate() in /Users/egortarasov/repos/Astor.Logging/Astor.Logging.Tests/ExceptionGenerator.cs:line 12",
  "logMessage": "fail of Egor { Name = Board Games, Favorite = Resistance }"
}
```



[`JsonConsoleFormatter.cs`](https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.Logging.Console/src/JsonConsoleFormatter.cs)

```csharp
private static void WriteItem(Utf8JsonWriter writer, KeyValuePair<string, object> item)
{
  string key = item.Key;
  switch (item.Value)
  {
    case bool flag:
      writer.WriteBoolean(key, flag);
      break;
    case byte num1:
      writer.WriteNumber(key, (int) num1);
      break;
    case sbyte num2:
      writer.WriteNumber(key, (int) num2);
      break;
    case char reference:
      writer.WriteString(key, (ReadOnlySpan<char>) MemoryMarshal.CreateSpan<char>(ref reference, 1));
      break;
    case Decimal num3:
      writer.WriteNumber(key, num3);
      break;
    case double num4:
      writer.WriteNumber(key, num4);
      break;
    case float num5:
      writer.WriteNumber(key, num5);
      break;
    case int num6:
      writer.WriteNumber(key, num6);
      break;
    case uint num7:
      writer.WriteNumber(key, num7);
      break;
    case long num8:
      writer.WriteNumber(key, num8);
      break;
    case ulong num9:
      writer.WriteNumber(key, num9);
      break;
    case short num10:
      writer.WriteNumber(key, (int) num10);
      break;
    case ushort num11:
      writer.WriteNumber(key, (int) num11);
      break;
    case null:
      writer.WriteNull(key);
      break;
    default:
      writer.WriteString(key, JsonConsoleFormatter.ToInvariantString(item.Value));
      break;
  }
}

private static string ToInvariantString(object obj)
{
  return Convert.ToString(obj, (IFormatProvider) CultureInfo.InvariantCulture);
}
```