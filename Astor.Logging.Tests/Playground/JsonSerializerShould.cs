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
        Exception ex;
        
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception e)
        {
            ex = e;
        }

        try
        {
            JsonSerializer.Serialize(ex);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}