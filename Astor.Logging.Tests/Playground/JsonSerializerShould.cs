using System;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Astor.Logging.Tests;

[TestClass]
public class JsonSerializerShould
{
    [TestMethod]
    [ExpectedException(typeof(NotSupportedException))]
    public void ThrowOnExceptionSerialization()
    {
        try
        {
            JsonSerializer.Serialize(ExceptionGenerator.Generate());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}