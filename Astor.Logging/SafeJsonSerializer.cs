using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Astor.Logging;

/// <summary>
/// Since System.Text.Json.JsonSerializer throws exception for certain types (including Exceptions)
/// The class wraps it with try-catch and uses Newtonsoft serializer instead.
/// </summary>
/// <param name="namingPolicy"></param>
/// <param name="indented"></param>
public class SafeJsonSerializer(JsonNamingPolicy namingPolicy, bool indented)
{
    readonly JsonSerializerOptions SystemSerializerOptions = new()
    {
        PropertyNamingPolicy = namingPolicy,
        DictionaryKeyPolicy = namingPolicy,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = indented
    };
        
    public readonly JsonSerializerSettings NewtonsoftSerializerSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = namingPolicy == JsonNamingPolicy.CamelCase ? new CamelCasePropertyNamesContractResolver() : new DefaultContractResolver(),
        Formatting = indented ? Formatting.Indented : Formatting.None,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };

    public string Serialize(object source)
    {
        try
        {
            return JsonSerializer.Serialize(source, SystemSerializerOptions);
        }
        catch
        {
            return JsonConvert.SerializeObject(source, NewtonsoftSerializerSettings);
        }
    }
}