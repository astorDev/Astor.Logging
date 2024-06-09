using System;

namespace Astor.Logging.Tests;

public static class PrintedExtensions
{
    public static T Printed<T>(this T obj)
    {
        Console.WriteLine(obj);
        return obj;
    }
}