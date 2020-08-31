using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace Astor.Logging
{
    public class RequestsLoggingMiddleware
    {
        public RequestDelegate Next { get; }
        public RequestsLoggingSettings Configuration { get; }
        public ILogger<RequestsLoggingMiddleware> Logger { get; }

        public RequestsLoggingMiddleware(RequestDelegate next
            , RequestsLoggingSettings configuration
            , ILogger<RequestsLoggingMiddleware> logger)
        {
            this.Next = next;
            this.Configuration = configuration;
            this.Logger = logger;
        }
        
        public async Task Invoke(HttpContext context)
        {
            if (this.Configuration.Ignores(context))
            {
                await this.Next(context);
                return;
            }

            var requestBody = await getRequestBodyAsync(context);
            var (elapsed, body) = await this.getResponseParamsAsync(context);
            
            var exceptionHandler = context.Features.Get<IExceptionHandlerPathFeature>();

            this.Logger.LogInformation(@"{uri}
{request}
{responseCode}
{responseBody}
{elapsed},
{exception}",
                context.Request.GetDisplayUrl(),
                requestBody,
                context.Response.StatusCode,
                body,
                elapsed, 
                exceptionHandler?.Error.ToString());
        }

        private async Task<(TimeSpan Elapsed, string Body)> getResponseParamsAsync(HttpContext context)
        {
            var stopwatch = new Stopwatch();

            string responseBody;

            var originalResponseStream = context.Response.Body;

            await using (var responseStream = new MemoryStream())
            {
                context.Response.Body = responseStream;

                stopwatch.Start();

                await this.Next(context);

                stopwatch.Stop();
                
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                
                responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
                
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                
                await responseStream.CopyToAsync(originalResponseStream);
            }

            return (stopwatch.Elapsed, responseBody);
        }

        private static async Task<string> getRequestBodyAsync(HttpContext context)
        {
            string requestBody;

            context.Request.EnableBuffering();

            await using (var ms = new MemoryStream())
            {
                await context.Request.Body.CopyToAsync(ms);
                ms.Position = 0;

                requestBody = await new StreamReader(ms).ReadToEndAsync();
            }

            context.Request.Body.Position = 0;

            return requestBody;
        }
    }
}