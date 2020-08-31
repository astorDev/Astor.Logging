using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LoggingExample.Protocol
{
    public class RestApiClient
    {
        public HttpClient HttpClient { get; }

        public RestApiClient(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
        }

        protected async Task<T> ReadAsync<T>(HttpResponseMessage response)
        {
            await this.OnResponseReceivedAsync(response);

            if (!response.IsSuccessStatusCode)
            {
                await this.OnNoneSuccessStatusCode(response);
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        protected virtual Task OnResponseReceivedAsync(HttpResponseMessage response)
        {
            return Task.CompletedTask;
        }

        protected virtual async Task OnNoneSuccessStatusCode(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
        }
    }
}