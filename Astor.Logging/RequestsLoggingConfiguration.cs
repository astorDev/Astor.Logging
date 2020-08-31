using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Astor.Logging
{
    public class RequestsLoggingSettings
    {
        public List<string> IgnoredPathPatterns { get; set; } = new List<string>();

        public bool Ignores(HttpContext context)
        {
            return this.IgnoredPathPatterns.Any(p => Regex.IsMatch(context.Request.Path.ToString(), p));
        }
    }

    public static class RequestsLoggingRegistration
    {
        public static void UseRequestsLogging(this IApplicationBuilder app, Action<RequestsLoggingSettings> configuration = null)
        {
            var settings = new RequestsLoggingSettings();
            configuration?.Invoke(settings);
            app.UseMiddleware<RequestsLoggingMiddleware>(settings);
        }
    }
}