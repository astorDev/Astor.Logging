using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Astor.Logging
{
    public class JsonConsoleLogger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (state is IEnumerable<KeyValuePair<string, object>> properties)
            {
                var dict = properties
                    .Where(k => k.Key != "{OriginalFormat}" && k.Value != null)
                    .ToDictionary(o => o.Key, o => o.Value);
                
                 Console.WriteLine(JsonConvert.SerializeObject(dict));
            }
        }
        
        public class Disposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}