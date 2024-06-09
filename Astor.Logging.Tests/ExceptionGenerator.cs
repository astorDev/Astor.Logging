using System;

namespace Astor.Logging.Tests;

public class ExceptionGenerator
{
    public static Exception Generate()
    {
        
        try
        {
            throw new InvalidOperationException($"Expected Exception number {Guid.NewGuid()}. ");
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}