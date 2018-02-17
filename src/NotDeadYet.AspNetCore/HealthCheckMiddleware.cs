using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NotDeadYet.Results;

namespace NotDeadYet.AspNetCore
{
    public class HealthCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHealthChecker _healthChecker;
        private readonly HealthCheckOptions _options;

        public HealthCheckMiddleware(
            RequestDelegate next,
            IHealthChecker healthChecker, HealthCheckOptions options)
        {
            _next = next;
            _healthChecker = healthChecker ?? throw new ArgumentNullException(nameof(healthChecker));
           _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            if (RequestHealthCheck(context.Request))
            {
                await ProcessRequest(context);
            }
            else
            {
                await _next(context);
            }
        }

        private bool RequestHealthCheck(HttpRequest request)
        {
            return request.Path.Equals(_options.Path, StringComparison.OrdinalIgnoreCase);
        }

        private async Task ProcessRequest(HttpContext context)
        {
            var result = _healthChecker.Check();

            var response = context.Response;
            var json = JsonConvert.SerializeObject(result, Formatting.Indented, new StringEnumConverter());

            response.StatusCode = result.Status == HealthCheckStatus.Okay ? 200 : 503;
            response.ContentType = "text/plain";
            response.Headers["Content-Disposition"] = "inline";
            response.Headers["Cache-Control"] = "no-cache";

            await response.WriteAsync(json);
        }
    }
}