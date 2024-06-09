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
        Exception ex;
        
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception e)
        {
            ex = e;
        }

        try
        {
            var json = JsonConvert.SerializeObject(ex);
            Console.WriteLine(json);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [TestMethod]
    public void SerializeObjectWithDeserializedObject()
    {
        var hobbyJson = """
                        {
                            "name" : "Board Games",
                            "favorite" : "Resistance"
                        }
                        """;

        var hobbyDeserializedWithSystem = System.Text.Json.JsonSerializer.Deserialize<object>(hobbyJson)!;
        var hobbyDeserializedWithConvert = JsonConvert.DeserializeObject(hobbyJson)!;

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