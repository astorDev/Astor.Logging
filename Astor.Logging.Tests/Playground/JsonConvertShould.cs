using System.Collections.Generic;
using Newtonsoft.Json;

namespace Astor.Logging.Tests;

[TestClass]
public class JsonConvertShould
{
    [TestMethod]
    public void SerializeExceptions()
    {
        JsonConvert.SerializeObject(ExceptionGenerator.Generate()).Printed();
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

        JsonConvert.SerializeObject(toSerialize, Formatting.Indented).Printed();
    }
}