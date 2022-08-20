using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Astor.Logging;

public class StateJsonConsoleLogger : ILogger
{
    readonly JsonSerializerOptions serializerOptions;

    public StateJsonConsoleLogger(JsonSerializerOptions serializerOptions)
    {
        this.serializerOptions = serializerOptions;
    }
    
    public IDisposable BeginScope<TState>(TState state) => new Disposable();
    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter) {
        if (state is not IEnumerable<KeyValuePair<string, object?>> properties) return;

        var props = properties.Where(k => k.Key != "{OriginalFormat}" && k.Value != null).ToDictionary(o => o.Key, o => o.Value);
        
        Console.WriteLine(JsonSerializer.Serialize(props, this.serializerOptions));
    }

    public class Disposable : IDisposable { public void Dispose() => GC.SuppressFinalize(this); }
    
    [ProviderAlias("StateJsonConsole")]
    public class Provider : ILoggerProvider {
        readonly JsonSerializerOptions serializerOptions;
        readonly ConcurrentDictionary<string, StateJsonConsoleLogger> loggers = new();

        public Provider(JsonSerializerOptions serializerOptions) { this.serializerOptions = serializerOptions; }
        
        public ILogger CreateLogger(string categoryName) => 
            this.loggers.GetOrAdd(categoryName, s => new(this.serializerOptions));

        public void Dispose() => GC.SuppressFinalize(this);
    }
}

public static class LoggingBuilderExtensions {
    public static ILoggingBuilder AddStateJsonConsole(this ILoggingBuilder loggingBuilder,
        Action<JsonSerializerOptions>? serializerOptionsConfig = null)
    {
        var options = new JsonSerializerOptions();
        serializerOptionsConfig?.Invoke(options);
        return loggingBuilder.AddProvider(new StateJsonConsoleLogger.Provider(options));
    }
}