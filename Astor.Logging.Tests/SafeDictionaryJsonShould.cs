using System;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Astor.Logging.Tests;

[TestClass]
public class SafeDictionaryJsonShould
{
    readonly SafeDictionaryJson StandardSerializer = new(new() { Indented = true });
    
    [TestMethod]
    public void SerializeException()
    {
        StandardSerializer.Serialize(new()
        {
            { "Exception", ExceptionGenerator.Generate() },
            { "Name", "Egor" },
        }).Printed();
    }

    [TestMethod]
    public void SerializeAnonymous()
    {
        StandardSerializer.Serialize(new()
        {
            { "Name", "Egor" },
            { "hobbyAnonymous", Hobby.Anonymous }
        }).Printed();
    }

    [TestMethod]
    public void SerializeDeserializedWithSystem()
    {
        var hobbySystemDeserialized = JsonSerializer.Deserialize<object>(Hobby.Json);
        StandardSerializer.Serialize(new()
        {
            { "Name", "Egor" },
            { "hobbySystemDeserializer", hobbySystemDeserialized! }
        });
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
        
        
        var hobbyNewtonsoftDeserialized = JsonConvert.DeserializeObject(Hobby.Json);
        
        serializer.Serialize(new()
        {
            { "Name", "Egor" },
            { "hobbySystemDeserializer", hobbyNewtonsoftDeserialized! }
        }).Printed();
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
        
        
        var hobbyNewtonsoftDeserialized = JsonConvert.DeserializeObject(Hobby.Json);
        var hobbySystemDeserialized = JsonSerializer.Deserialize<object>(Hobby.Json);
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