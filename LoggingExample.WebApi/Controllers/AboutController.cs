using System;
using System.Security;
using LoggingExample.Protocol;
using LoggingExample.Protocol.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LoggingExample.WebApi.Controllers
{
    [Route(Uris.About)]
    public class AboutController
    {
        public IWebHostEnvironment Environment { get; }
        
        public AboutController(IWebHostEnvironment environment)
        {
            this.Environment = environment;
        }

        [HttpGet]
        public About Get()
        {
            return new About
            {
                Description = "LoggingExample - template API",
                Environment = this.Environment.EnvironmentName,
                Version = this.GetType().Assembly.GetName().Version.ToString()
            };
        }
    }
}