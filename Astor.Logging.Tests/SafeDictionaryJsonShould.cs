using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Astor.Logging.Tests;

[TestClass]
public class SafeDictionaryJsonShould
{
    string hobbyJson = """
                       {
                        "Name" : "Games",
                        "fav" : "Ticket to ride"
                       }
                       """;
    
    
    [TestMethod]
    public void SerializeException()
    {
        var serializer = new SafeDictionaryJson(new() { Indented = true });

        var json = serializer.Serialize(new()
        {
            { "Exception", ExceptionGenerator.Generate() },
            { "Name", "Egor" },
        });

        Console.WriteLine(json);

    }

    [TestMethod]
    public void SerializeAnonymous()
    {
        var serializer = new SafeDictionaryJson(new() { Indented = true });
        
        var json = serializer.Serialize(new()
        {
            { "Name", "Egor" },
            { "hobbyAnonymous", Hobby.Anonymous }
        });
        
        Console.WriteLine(json);
    }

    [TestMethod]
    public void SerializeDeserializedWithSystem()
    {
        var serializer = new SafeDictionaryJson(new() { Indented = true });
        var hobbySystemDeserialized = JsonSerializer.Deserialize<object>(hobbyJson);
        
        var json = serializer.Serialize(new()
        {
            { "Name", "Egor" },
            { "hobbySystemDeserializer", hobbySystemDeserialized! }
        });
        
        Console.WriteLine(json);
    }
    
    [TestMethod]
    public void SerializeDeserializedWithNewtonsoft()
    {
        var serializer = new SafeDictionaryJson(new()
        {
            Indented = true,
            CustomTypeWriters =
            {
                { JObjectWriter.KeyValuePair.Key, JObjectWriter.KeyValuePair.Value }
            }
        });
        
        
        var hobbyNewtonsoftDeserialized = JsonConvert.DeserializeObject(hobbyJson);
        
        var json = serializer.Serialize(new()
        {
            { "Name", "Egor" },
            { "hobbySystemDeserializer", hobbyNewtonsoftDeserialized! }
        });
        
        Console.WriteLine(json);
    }

    public void SerializeReal()
    {
        
    }

    [TestMethod]
    public void SerializeCombined()
    {
        var serializer = new SafeDictionaryJson(new()
        {
            Indented = true,
            CustomTypeWriters =
            {
                { JObjectWriter.KeyValuePair.Key, JObjectWriter.KeyValuePair.Value }
            }
        });
        
        
        var hobbyNewtonsoftDeserialized = JsonConvert.DeserializeObject(hobbyJson);
        var hobbySystemDeserialized = JsonSerializer.Deserialize<object>(hobbyJson);
        var hobbyAnonymous = new
        {
            Name = "Board Games",
            fav = "Resistance"
        };
        
        var json = serializer.Serialize(new()
        {
            { "Name", "Egor" },
            { "hobbyNewtonsoftDeserializer", hobbyNewtonsoftDeserialized! },
            { "hobbySystemDeserializer", hobbySystemDeserialized! },
            { "hobbyAnonymous", hobbyAnonymous }
        });
        
        Console.WriteLine(json);
    }
    
}