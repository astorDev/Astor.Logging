using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Astor.Logging.Tests;

[TestClass]
public class JsonConvertShould
{
    [TestMethod]
    public void SerializeExceptions()
    {
        var json = JsonConvert.SerializeObject(ExceptionGenerator.Generate());
        Console.WriteLine(json);
    }

    [TestMethod]
    public void SerializeObjectWithDeserializedObject()
    {
        var hobbyDeserializedWithSystem = System.Text.Json.JsonSerializer.Deserialize<object>(Hobby.Json)!;
        var hobbyDeserializedWithConvert = JsonConvert.DeserializeObject(Hobby.Json)!;

        var toSerialize = new Dictionary<string, object>()
        {
            { "name", "Egor" },
            { "hobbySystem", hobbyDeserializedWithSystem },
            { "hobbyNewtonsoft", hobbyDeserializedWithConvert }
        };

        var json = JsonConvert.SerializeObject(toSerialize, Formatting.Indented);
        Console.WriteLine(json);
    }
}