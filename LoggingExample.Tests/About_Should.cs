using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoggingExample.Tests
{
    [TestClass]
    public class About_Should : Test
    {
        [TestMethod]
        public async Task ReturnValidMetadata()
        {
            var client = this.Factory.Create();

            var about = await client.GetAboutAsync();

            Assert.AreEqual("LoggingExample - template API", about.Description);
            Assert.AreEqual("1.0.0.0", about.Version);
            Assert.AreEqual("Development", about.Environment);
        }
    }
}