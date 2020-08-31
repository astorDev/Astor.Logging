using System;
using System.Net.Http;
using System.Threading.Tasks;
using LoggingExample.Protocol;
using LoggingExample.WebApi;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LoggingExample.Tests
{
    public class WebApplicationFactory : WebApplicationFactory<Startup>
    {
        private HttpClient httpClient;

        private void ensureHttpClientCreated()
        {
            if (this.httpClient == null)
            {
                this.httpClient = this.CreateClient();
            }
        }

        public IServiceProvider ServiceProvider
        {
            get
            {
                this.ensureHttpClientCreated();
                return this.Server.Host.Services;
            }
        }

        public MyClient Create()
        {
            this.ensureHttpClientCreated();
            return new TestClient(this.httpClient);
        }

        private class TestClient : MyClient
        {
            public TestClient(HttpClient httpClient) : base(httpClient)
            {
            }
        }
    }
}