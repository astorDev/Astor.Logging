using System.Net.Http;
using System.Threading.Tasks;
using LoggingExample.Protocol.Models;

namespace LoggingExample.Protocol
{
    public class MyClient : RestApiClient
    {
        public MyClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<About> GetAboutAsync()
        {
            var response = await this.HttpClient.GetAsync(Uris.About);
            return await this.ReadAsync<About>(response);
        }
    }
}