using System;
using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace Astor.Logging;

public class JObjectWriter
{
    public static KeyValuePair<Type, Action<Utf8JsonWriter, KeyValuePair<string, object>>> KeyValuePair =
        new(
            typeof(JObject),
            (writer, pair) =>
            {
                var jObj = (JObject)pair.Value;

                writer.WritePropertyName(pair.Key);
                writer.WriteRawValue(jObj.ToString());
            }
        ); 
}