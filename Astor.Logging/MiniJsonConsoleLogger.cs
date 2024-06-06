using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Astor.Logging;

public class MiniJsonConsoleLogger(MiniJsonConsoleLogger.Options options) : ILogger
{
    public const string Id = "MiniJsonConsole";
    
    public IDisposable BeginScope<TState>(TState state) => new Disposable();
    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter)
    {
        if (state is not IEnumerable<KeyValuePair<string, object?>> rawStateProps) return; // sanity check
        var notNullStateProps = rawStateProps.Where(x => x.Value != null).ToDictionary(o => o.Key, o => o.Value);

        var props = new Dictionary<string, object>();
        
        if (options.Includes.HasFlag(LogParts.State))
        {
            var stateProps = notNullStateProps.Where(k => k.Key != "{OriginalFormat}" && k.Value != null).ToDictionary(o => o.Key, o => o.Value!);
            props = props.Union(stateProps).ToDictionary(x => x.Key, x => x.Value);
        }

        if (options.Includes.HasFlag(LogParts.LogOriginalFormat))
        {
            var originalFormatProp = notNullStateProps.Single(p => p.Key == "{OriginalFormat}");
            props.TryAdd(nameof(LogParts.LogOriginalFormat), originalFormatProp.Value!);
        }

        if (options.Includes.HasFlag(LogParts.LogCategoryName))
        {
            props.TryAdd(nameof(LogParts.LogCategoryName), options.CategoryName);
        }

        if (options.Includes.HasFlag(LogParts.LogLevel))
        {
            props.TryAdd(nameof(LogParts.LogLevel), logLevel);
        }

        if (options.Includes.HasFlag(LogParts.LogEventId))
        {
            props.TryAdd(nameof(LogParts.LogEventId), logLevel);
        }

        var json = options.Serializer.Serialize(props);
        Console.WriteLine(json);
    }

    public class Disposable : IDisposable { public void Dispose() => GC.SuppressFinalize(this); }

    public class OptionsBuilder
    {
        bool indented = false;
        JsonNamingPolicy namingPolicy = JsonNamingPolicy.CamelCase;
        LogParts includes = LogParts.State;

        public OptionsBuilder Indent()
        {
            indented = true;
            return this;
        }

        public OptionsBuilder SetNamingPolicy(JsonNamingPolicy namingPolicy)
        {
            this.namingPolicy = namingPolicy;
            return this;
        }
        
        public OptionsBuilder IncludeAll()
        {
            includes = LogParts.State | LogParts.LogOriginalFormat | LogParts.LogCategoryName | LogParts.LogLevel | LogParts.LogEventId;
            return this;
        }

        public Options Build(string categoryName) => new(categoryName, includes, new(namingPolicy, indented));
    }
    
    public record Options(string CategoryName, LogParts Includes, SafeJsonSerializer Serializer);
    
    [ProviderAlias(Id)]
    public class Provider(OptionsBuilder optionsBuilder) : ILoggerProvider {
        
        readonly ConcurrentDictionary<string, MiniJsonConsoleLogger> loggers = new();
        
        public ILogger CreateLogger(string categoryName) => 
            this.loggers.GetOrAdd(categoryName, c => new(optionsBuilder.Build(c)));

        public void Dispose() => GC.SuppressFinalize(this);
    }
}

[Flags]
public enum LogParts
{
    State = 1,
    LogOriginalFormat = 2,
    LogCategoryName = 4,
    LogLevel = 8,
    LogEventId = 16
}


public static class LoggingBuilderExtensions {
    public static ILoggingBuilder AddMiniJsonConsole(this ILoggingBuilder loggingBuilder,
        Action<MiniJsonConsoleLogger.OptionsBuilder>? config = null)
    {
        var options = new MiniJsonConsoleLogger.OptionsBuilder();
        config?.Invoke(options);
        return loggingBuilder.AddProvider(new MiniJsonConsoleLogger.Provider(options));
    }
}