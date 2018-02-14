using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NotDeadYet.Results;

namespace NotDeadYet.Samples.AspNetCore
{
    public class HealthCheckMiddleware
    {
        private const string ApplicationText = "application/json"; // For backward compatibility with earlier version of NotDeadYet

        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IHealthChecker _healthChecker;

        private readonly JsonSerializer _jsonSerializer;
        private readonly JsonSerializer serializer = new JsonSerializer();


        public HealthCheckMiddleware(
            RequestDelegate next,
            IHealthChecker healthChecker)
        {

            _next = next;
            _healthChecker = healthChecker ?? throw new ArgumentNullException(nameof(healthChecker));
        }

        public async Task Invoke(HttpContext context)
        {
            //TODO: Add check to make sure path starts with /
            //TODO: Perform benchmarks
            //TODO: Investigate streaming response.
            string path = "/healthcheck";

            PathString subpath;
            if (!context.Request.Path.StartsWithSegments(path, out subpath))
            {
                await _next(context);
                return;
            }


            var result = _healthChecker.Check();

            var response = context.Response;
            var json = JsonConvert.SerializeObject(result, Formatting.Indented, new StringEnumConverter());

            response.StatusCode = result.Status == HealthCheckStatus.Okay ? 200 : 503;
            response.ContentType = "text/plain";
            response.Headers["Content-Disposition"] = "inline";
            response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            response.Headers.Add("Pragma", "no-cache");
            await context.Response.WriteAsync(json);

        }
    }
}