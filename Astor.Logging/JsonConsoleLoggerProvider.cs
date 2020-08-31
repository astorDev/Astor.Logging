using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Astor.Logging
{
    public class JsonConsoleLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, JsonConsoleLogger> loggers = new ConcurrentDictionary<string, JsonConsoleLogger>();
        
        public ILogger CreateLogger(string categoryName)
        {
            return this.loggers.GetOrAdd(categoryName, s => new JsonConsoleLogger());
        }

        public void Dispose()
        {
        }
    }
    
    public static class JsonConsoleLoggerProviderExtensions 
    {
        public static void AddJsonConsole(this ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.AddProvider(new JsonConsoleLoggerProvider());
        }
    }
}